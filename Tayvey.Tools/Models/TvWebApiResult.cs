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
    public class TvWebApiResult : IActionResult
    {
        /// <summary>
        /// 响应头
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string>? Header { get; set; }

        /// <summary>
        /// 相应类型
        /// </summary>
        [JsonIgnore]
        public string? ContentType { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public TvApiStatus Status { get; private set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// 数据量
        /// </summary>
        public long? Total { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="status"></param>
        public TvWebApiResult(TvApiStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// WebApi返回处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            // 设置常规响应头
            response.StatusCode = Status.GetHashCode();
            if (!string.IsNullOrWhiteSpace(ContentType))
            {
                response.ContentType = ContentType;
            }

            // 设置自定义响应头
            if (Header != null && Header.Count > 0)
            {
                Parallel.ForEach(Header, header =>
                {
                    response.Headers[header.Key] = header.Value;
                });
            }

            // 返回
            return response.WriteAsync(JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver() // 首字母小写
            }));
        }
    }
}