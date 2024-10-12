using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Service
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
        /// 异常处理
        /// </summary>
        private readonly Action<Exception, HttpContext>? _exProcess;

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="next">委托</param>
        /// <param name="exProcess">异常处理</param>
        public TvGlobalExMiddleware(RequestDelegate next, Action<Exception, HttpContext>? exProcess = null)
        {
            _next = next;
            _exProcess = exProcess;
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
                if (_exProcess != null)
                {
                    _exProcess(e, context);
                }
                else
                {
                    var response = context.Response;

                    response.StatusCode = TvApiStatus.Error.GetHashCode();
                    response.ContentType = "application/json; charset=utf-8";

                    var result = new TvWebApiResult(TvApiStatus.Error, e.Message);

                    await response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(), // 首字母小写
                        NullValueHandling = NullValueHandling.Ignore // 为NULL的字段过滤
                    }));
                }
            }
        }
    }
}