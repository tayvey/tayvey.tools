#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tayvey.Tools.TvApiResults.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
#endif

namespace Tayvey.Tools.TvExceptions.Middlewares
{
    /// <summary>
    /// TvWebApi全局异常处理中间件
    /// </summary>
    public sealed class TvExWebApiGlobalExMiddleware
    {
        /// <summary>
        /// 请求委托
        /// </summary>
        private readonly RequestDelegate? Next;

        /// <summary>
        /// 自定义处理异常委托
        /// </summary>
        private readonly Action<Exception>? CustomProcessEx;

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">请求委托</param>
        /// <param name="customProcessEx">自定义处理异常委托</param>
        public TvExWebApiGlobalExMiddleware(RequestDelegate next, Action<Exception>? customProcessEx = null)
        {
            Next = next;
            CustomProcessEx = customProcessEx;
        }

        /// <summary>
        /// 私有空构造
        /// </summary>
        private TvExWebApiGlobalExMiddleware() { }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 调用下一个中间件
                if (Next != null)
                {
                    await Next.Invoke(context);
                }
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                {
                    throw;
                }

                if (CustomProcessEx != null)
                {
                    await Task.Run(() => CustomProcessEx(e));
                }

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

    /// <summary>
    /// TvWebApi全局异常处理
    /// </summary>
    public static class TvExWebApiGlobalEx
    {
        /// <summary>
        /// 使用TvWebApi全局异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="customProcessEx">自定义处理异常委托</param>
        public static void UseTvExWebApiGlobalEx(this IApplicationBuilder app, Action<Exception>? customProcessEx = null)
        {
            app.UseMiddleware<TvExWebApiGlobalExMiddleware>(customProcessEx);
        }
    }
}