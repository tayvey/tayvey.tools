using Microsoft.AspNetCore.Builder;

namespace Tayvey.Tools
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
        public static void UseTvGlobalEx(this IApplicationBuilder app)
        {
            app.UseMiddleware<TvGlobalExMiddleware>();
        }
    }
}