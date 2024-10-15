using Microsoft.AspNetCore.Mvc;
using System.IO;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public abstract class TvControllerBase : ControllerBase
    {
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private static TvWebApiResult Return(TvApiStatus status, string? message = null, object? data = null, long? total = null) => new TvWebApiResult(status)
        {
            ContentType = "application/json; charset=utf-8",
            Message = message,
            Data = data,
            Total = total
        };

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvOk(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.Ok, message, data, total);

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvFail(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.Fail, message, data, total);

        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvUnauthorized(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.Unauthorized, message, data, total);

        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvNotFound(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.NotFound, message, data, total);

        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvMethodNotAllowed(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.MethodNotAllowed, message, data, total);

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static TvWebApiResult TvError(string? message = null, object? data = null, long? total = null) => Return(TvApiStatus.Error, message, data, total);

        /// <summary>
        /// 返回文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TvWebApiStreamResult TvFile(Stream stream, string fileName) => new TvWebApiStreamResult(stream, fileName)
        {
            ContentType = "application/octet-stream"
        };
    }
}