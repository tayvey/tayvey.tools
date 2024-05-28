#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
#endif

namespace Tayvey.Tools.TvMiddlewares.Middlewares
{
    /// <summary>
    /// Tv中间件抽象类
    /// </summary>
#if NET8_0_OR_GREATER
    /// <param name="next">请求委托</param>
    public abstract class TvMiddlewareBase(RequestDelegate next)
#else
    public abstract class TvMiddlewareBase
#endif
    {
        /// <summary>
        /// 请求委托
        /// </summary>
#if NET8_0_OR_GREATER
        protected readonly RequestDelegate Next = next;
#else
        protected readonly RequestDelegate Next;
#endif

#if NET6_0 || NETSTANDARD2_1
        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">请求委托</param>
        protected TvMiddlewareBase(RequestDelegate next)
        {
            Next = next;
        }
#endif

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public abstract Task InvokeAsync(HttpContext context);
    }
}