using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Helpers;

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
                _ = lc switch
                {
                    TvAutoDILifeCycle.Scoped when i == null => services.AddScoped(autoDI),
                    TvAutoDILifeCycle.Scoped when i != null => services.AddScoped(i, autoDI),
                    TvAutoDILifeCycle.Singleton when i == null => services.AddSingleton(autoDI),
                    TvAutoDILifeCycle.Singleton when i != null => services.AddSingleton(i, autoDI),
                    _ when i == null => services.AddTransient(autoDI),
                    _ => services.AddTransient(i, autoDI),
                };
            }
        }

        /// <summary>
        /// 获取自动依赖注入的服务
        /// </summary>
        /// <returns></returns>
        private static List<(Type autoDI, TvAutoDILifeCycle lifeCycle, Type? i)> GetAutoDIs()
        {
            var result = new List<(Type autoDI, TvAutoDILifeCycle lifeCycle, Type? i)>();

            foreach (var loadedType in TvAssembly.GetLoadedAssemblies())
            {
                var attr = loadedType.GetCustomAttribute<TvAutoDIAttribute>();
                if (!loadedType.IsClass || loadedType.IsAbstract || attr == null)
                {
                    continue;
                }

                var allInterfaces = loadedType.GetInterfaces();
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(x => x.GetInterfaces());
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();

                if (interfaces.Count == 0)
                {
                    result.Add((loadedType, attr._lifeCycle, null));
                }
                else
                {
                    result.AddRange(interfaces.Select(i => (loadedType, attr._lifeCycle, (Type?)i)));
                }
            }

            return result;
        }
    }
}