using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// WebApi文件流返回对象
    /// </summary>
    public class TvWebApiStreamResult : IActionResult
    {
        /// <summary>
        /// 响应头
        /// </summary>
        public Dictionary<string, string>? Header { get; set; }

        /// <summary>
        /// 相应类型
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public Stream FileStream { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public TvWebApiStreamResult(Stream stream, string fileName)
        {
            FileStream = stream;
            FileName = fileName;
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~TvWebApiStreamResult()
        {
            try
            {
                FileStream.Dispose();
            }
            catch{}
        }

        /// <summary>
        /// WebApi返回处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            // 设置常规响应头
            response.StatusCode = TvApiStatus.Ok.GetHashCode();
            if (!string.IsNullOrWhiteSpace(ContentType))
            {
                response.ContentType = ContentType;
            }
            response.Headers["Content-Disposition"] = $"attachment; filename={FileName}";

            // 设置自定义响应头
            if (Header != null && Header.Count > 0)
            {
                Parallel.ForEach(Header, header =>
                {
                    response.Headers[header.Key] = header.Value;
                });
            }

            // 返回
            using (FileStream)
            {
                FileStream.Position = 0;
                await FileStream.CopyToAsync(response.Body);
            }
        }
    }
}