using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Tayvey.Tools
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public abstract class TvControllerBase : ControllerBase
    {
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static ObjectResult Return(int statusCode, string message, object? data = null) => new ObjectResult(new
        {
            StatusCode = statusCode,
            Message = message,
            Data = data
        })
        {
            StatusCode = statusCode
        };

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvOk(string message, object? data = null) => Return(200, message, data);

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvFail(string message, object? data = null) => Return(400, message, data);

        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvUnauthorized(string message, object? data = null) => Return(401, message, data);

        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvNotFound(string message, object? data = null) => Return(404, message, data);

        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvMethodNotAllowed(string message, object? data = null) => Return(405, message, data);

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult TvError(string message, object? data = null) => Return(500, message, data);

        /// <summary>
        /// 返回文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IActionResult TvFile(Stream stream, string fileName) => File(stream, "application/octet-stream", fileName);
    }
}