using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools
{
    /// <summary>
    /// 自动依赖注入
    /// </summary>
    public static class TvAutoDI
    {
        /// <summary>
        /// 添加Tv自动依赖注入
        /// </summary>
        /// <param name="service"></param>
        public static void AddTvAutoDI(this IServiceCollection service)
        {
            // 获取自动依赖注入的类
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(i => i.GetTypes())
                .SelectMany(i => i)
                .Where(i => i.IsClass && !i.IsAbstract)
                .Where(i => i.GetCustomAttribute<TvAutoDIAttribute>() != null)
                .Select(i =>
                {
                    var attr = i.GetCustomAttribute<TvAutoDIAttribute>()!;

                    var allInterfaces = i.GetInterfaces();
                    var baseInterfaces = allInterfaces.AsParallel().SelectMany(x => x.GetInterfaces());
                    var interfaces = allInterfaces.Except(baseInterfaces).ToList();

                    return interfaces.Select(x => (i, attr._lifeCycle, x)).ToList();
                })
                .SelectMany(i => i)
                .ToList();

            // 遍历依赖注入
            foreach (var (i, lc, c) in list)
            {
                if (lc == TvAutoDILifeCycle.Scoped)
                {
                    service.AddScoped(c, i);
                    continue;
                }

                if (lc == TvAutoDILifeCycle.Singleton)
                {
                    service.AddSingleton(c, i);
                    continue;
                }

                service.AddTransient(c, i);
            }
        }
    }
}