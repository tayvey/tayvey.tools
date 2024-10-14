using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// 自动注册中间件扩展
    /// </summary>
    public static class TvAutoMwEx
    {
        /// <summary>
        /// 使用自动注册中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="marks"></param>
        public static void UseTvAutoMw(this IApplicationBuilder app, params string[] marks)
        {
            // 遍历注册中间件
            foreach (var (middleware, _) in GetMiddlewares(marks).OrderByDescending(i => i.sort))
            {
                app.UseMiddleware(middleware);
            }
        }

        /// <summary>
        /// 获取自动注册的中间件
        /// </summary>
        /// <returns></returns>
        private static List<(Type middleware, uint sort)> GetMiddlewares(string[] marks)
        {
            var result = new List<(Type middleware, uint sort)>();

            foreach (var loadedType in TvAssemblyEx.LoadedTypes)
            {
                var attr = loadedType.GetCustomAttribute<TvAutoMwAttribute>();
                if (!loadedType.IsClass || loadedType.IsAbstract || attr == null)
                {
                    continue;
                }

                if (attr._marks.Length > 0 && !marks.Any(x => attr._marks.Contains(x)))
                {
                    continue;
                }

                result.Add((loadedType, attr._sort));
            }

            return result;
        }
    }
}