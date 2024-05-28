#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tayvey.Tools.TvApiResults.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
#endif

namespace Tayvey.Tools.TvMiddlewares.Middlewares
{
    /// <summary>
    /// Tv中间件全局异常
    /// </summary>
#if NET8_0_OR_GREATER
    /// <param name="next">请求委托</param>
    public abstract class TvMiddlewareGlobalEx(RequestDelegate next) : TvMiddlewareBase(next)
#else
    public abstract class TvMiddlewareGlobalEx : TvMiddlewareBase
#endif
    {
        /// <summary>
        /// 自定义处理异常委托
        /// </summary>
        public Action<Exception>? Process { get; set; }

#if NET6_0 || NETSTANDARD2_1
        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">请求委托</param>
        public TvMiddlewareGlobalEx(RequestDelegate next) : base(next)
        {
        }
#endif

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            // 捕获请求管道异常
            try
            {
                // 调用下一个中间件
                await Next(context);
            }
            catch (Exception e)
            {
                // 调试模式直接抛出异常
                if (Debugger.IsAttached) throw;

                // 自定义处理异常
                if (Process != null)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            Process(e);
                        }
                        catch{}
                    });
                }

                // 返回异常
                await ReturnError(context);
            }
        }

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        private static async Task ReturnError(HttpContext context)
        {
            // 返回对象
            var result = TvApiResult.Error("未知的异常");

            // 自定义返回
            var response = context.Response;
            response.ContentType = "application/json; charset=utf-8";

#if NET6_0_OR_GREATER
            response.Headers.Server = "";
            response.Headers.Append("Tv-Api-Result", "true");
#else
            response.Headers["Server"] = "";
            response.Headers["Tv-Api-Result"] = "true";
#endif

            response.StatusCode = result.StatusCode.GetHashCode();

            await response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}