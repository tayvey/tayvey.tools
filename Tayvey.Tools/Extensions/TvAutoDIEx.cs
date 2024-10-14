using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// 自动依赖注入扩展
    /// </summary>
    public static class TvAutoDIEx
    {
        /// <summary>
        /// 添加自动依赖注入服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTvAutoDI(this IServiceCollection services)
        {
            foreach (var (autoDI, lc, i) in GetAutoDIs())
            {
                if (lc == TvAutoDILifeCycle.Scoped)
                {
                    services.AddScoped(autoDI, i);
                    continue;
                }

                if (lc == TvAutoDILifeCycle.Singleton)
                {
                    services.AddSingleton(autoDI, i);
                    continue;
                }

                services.AddTransient(autoDI, i);
            }
        }

        /// <summary>
        /// 获取自动依赖注入的服务
        /// </summary>
        /// <returns></returns>
        private static List<(Type autoDI, TvAutoDILifeCycle lifeCycle, Type i)> GetAutoDIs()
        {
            var result = new List<(Type autoDI, TvAutoDILifeCycle lifeCycle, Type i)>();

            foreach (var loadedType in TvAssemblyEx.LoadedTypes)
            {
                var attr = loadedType.GetCustomAttribute<TvAutoDIAttribute>();
                if (!loadedType.IsClass || loadedType.IsAbstract || attr == null)
                {
                    continue;
                }

                var allInterfaces = loadedType.GetInterfaces();
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(x => x.GetInterfaces());
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();

                result.AddRange(allInterfaces.Except(baseInterfaces).Select(i => (loadedType, attr._lifeCycle, i)));
            }

            return result;
        }
    }
}