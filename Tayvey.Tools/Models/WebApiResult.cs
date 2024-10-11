using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Tayvey.Tools.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// WebApi返回对象
    /// </summary>
    public class WebApiResult<T> : IActionResult
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public ApiStatus Status { get; private set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 响应头
        /// </summary>
        //[JsonIgnore]
        public Dictionary<string, string> Header { get; set; }

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public WebApiResult(ApiStatus status, string message, Dictionary<string, string>? header = null) 
        {
            Status = status;
            Message = message;
            Header = (header ??= new Dictionary<string, string>());
        }

        /// <summary>
        /// 私有空构造
        /// </summary>
        private WebApiResult() { }

        /// <summary>
        /// WebApi返回处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json; charset=utf-8";
            response.StatusCode = Status.GetHashCode();
            response.Headers["Server"] = "";

            Parallel.ForEach(Header, header =>
            {
                response.Headers[header.Key] = header.Value;
            });

            return response.WriteAsync(JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // 首字母小写
                NullValueHandling = NullValueHandling.Ignore // 为NULL的字段过滤
            }));
        }
    }
}