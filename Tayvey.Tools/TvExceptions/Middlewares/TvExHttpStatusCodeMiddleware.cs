#if NETSTANDARD2_1
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Tayvey.Tools.TvApiResults.Models;
#endif

namespace Tayvey.Tools.TvExceptions.Middlewares
{
    /// <summary>
    /// TvHttp状态码处理中间件
    /// </summary>
    public sealed class TvExHttpStatusCodeMiddleware
    {
        /// <summary>
        /// 请求委托
        /// </summary>
        private readonly RequestDelegate? Next;

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">请求委托</param>
        public TvExHttpStatusCodeMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        /// <summary>
        /// 私有空构造
        /// </summary>
        private TvExHttpStatusCodeMiddleware() { }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // 调用下一个中间件
            if (Next != null)
            {
                await Next.Invoke(context);
            }

            // TvApiResult返回不做处理
            if (context.Response.Headers.ContainsKey("Tv-Api-Result"))
            {
                return;
            }

            // 返回对象
            var result = context.Response.StatusCode switch
            {
                404 => TvApiResult.NotFound("请求资源不存在"),
                405 => TvApiResult.MethodNotAllowed("方法不被允许"),
                _ => null
            };

            // 自定义返回
            if (result != null)
            {
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
    /// TvHttp状态码处理
    /// </summary>
    public static class TvExHttpStatusCode
    {
        /// <summary>
        /// 使用TvHttp状态码处理中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseTvExHttpStatusCode(this IApplicationBuilder app)
        {
            app.UseMiddleware<TvExHttpStatusCodeMiddleware>();
        }
    }
}