#if NETSTANDARD2_1
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvMiddlewares.Middlewares;
#endif

namespace Tayvey.Tools.TvMiddlewares
{
    /// <summary>
    /// Tv中间件
    /// </summary>
    public static class TvMiddleware
    {
        /// <summary>
        /// 自动注册的中间件
        /// </summary>
#if NET6_0_OR_GREATER
        private static readonly Lazy<List<Type>> Classes = new(LzClasses);
#else
        private static readonly Lazy<List<Type>> Classes = new Lazy<List<Type>>(LzClasses);
#endif

        /// <summary>
        /// 懒加载自动注册的中间件
        /// </summary>
        /// <returns></returns>
        private static List<Type> LzClasses() => TvRelyAssembly.LzRelyAssemblyTypes.Value.AsParallel()
            .Where(i => i.IsClass && !i.IsAbstract && i.IsSubclassOf(typeof(TvMiddlewareBase)))
            .ToList();

        /// <summary>
        /// 使用Tv中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTvMiddleware(this IApplicationBuilder app)
        {
            // 遍历配置
            foreach (var item in TvConfig.Options.TvMiddleware)
            {
                // 匹配中间件
                var type = Classes.Value.FirstOrDefault(i => i.Name == item);
                if (type == null)
                {
                    continue;
                }

                // 注册
                app.UseMiddleware(type);
            }

            return app;
        }
    }
}