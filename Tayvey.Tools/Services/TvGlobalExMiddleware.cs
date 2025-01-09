using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Tayvey.Tools
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    internal class TvGlobalExMiddleware
    {
        /// <summary>
        /// 委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">委托</param>
        public TvGlobalExMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = context.Response;

                response.StatusCode = 500;
                response.ContentType = "application/json; charset=utf-8";

                var result = TvControllerBase.TvError("系统异常, 请联系管理员");

                Console.WriteLine($"{e.Message}\n{e.StackTrace}");

                await response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver() // 首字母小写
                }));
            }
        }
    }
}