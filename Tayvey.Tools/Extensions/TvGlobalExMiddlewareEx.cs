using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using Tayvey.Tools.Services;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// 全局异常处理中间件扩展
    /// </summary>
    public static class TvGlobalExMiddlewareEx
    {
        /// <summary>
        /// 使用全局异常中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="exProcess"></param>
        public static void UseTvGlobalEx(this IApplicationBuilder app, Action<Exception, HttpContext>? exProcess = null)
        {
            if (exProcess != null)
            {
                app.UseMiddleware<TvGlobalExMiddleware>(exProcess);
            }
            else
            {
                app.UseMiddleware<TvGlobalExMiddleware>();
            }
        }
    }
}
