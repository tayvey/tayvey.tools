#if NETSTANDARD2_1
using Tayvey.Tools.TvApiResults.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
#endif

namespace Tayvey.Tools.TvApiResults.Models
{
    /// <summary>
    /// TvApi返回对象
    /// </summary>
    public sealed class TvApiResult : IActionResult
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public TvApiStatus StatusCode { get; private set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string? Message { get; private set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object? Data { get; private set; }

        /// <summary>
        /// 私有构造, 不允许初始化
        /// </summary>
        private TvApiResult() { }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult Ok(string? message = null, object? data = null) => new()
#else
        public static TvApiResult Ok(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.Ok,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult Fail(string? message = null, object? data = null) => new()
#else
        public static TvApiResult Fail(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.Fail,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 鉴权失败
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult Unauthorized(string? message = null, object? data = null) => new()
#else
        public static TvApiResult Unauthorized(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.Unauthorized,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 资源不存在
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult NotFound(string? message = null, object? data = null) => new()
#else
        public static TvApiResult NotFound(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.NotFound,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 方法不被允许
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult MethodNotAllowed(string? message = null, object? data = null) => new()
#else
        public static TvApiResult MethodNotAllowed(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.MethodNotAllowed,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static TvApiResult Error(string? message = null, object? data = null) => new()
#else
        public static TvApiResult Error(string? message = null, object? data = null) => new TvApiResult
#endif
        {
            StatusCode = TvApiStatus.Error,
            Message = message,
            Data = data
        };

        /// <summary>
        /// WebApi返回处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json; charset=utf-8";

#if NET6_0_OR_GREATER
            response.Headers.Server = "";
            response.Headers.Append("Tv-Api-Result", "true");
#else
            response.Headers["Server"] = "";
            response.Headers["Tv-Api-Result"] = "true";
#endif

            response.StatusCode = StatusCode.GetHashCode();

            return response.WriteAsync(JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}