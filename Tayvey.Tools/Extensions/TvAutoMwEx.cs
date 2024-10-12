using Microsoft.AspNetCore.Builder;
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
            // 获取自动注册的中间件
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(i => i.GetTypes())
                .SelectMany(i => i)
                .Where(i => i.IsClass && !i.IsAbstract && i.GetCustomAttribute<TvAutoMwAttribute>() != null)
                .Select(i =>
                {
                    var attr = i.GetCustomAttribute<TvAutoMwAttribute>()!;

                    if (attr._marks.Length == 0)
                    {
                        return new
                        {
                            Mw = i,
                            Sort = attr._sort
                        };
                    }

                    if (!marks.Any(x => attr._marks.Contains(x)))
                    {
                        return null;
                    }

                    return new
                    {
                        Mw = i,
                        Sort = attr._sort
                    };
                })
                .Where(i => i != null)
                .Select(i => i!)
                .ToList();

            // 遍历注册中间件
            foreach (var item in list.OrderByDescending(i => i.Sort).ToList())
            {
                app.UseMiddleware(item.Mw);
            }
        }
    }
}