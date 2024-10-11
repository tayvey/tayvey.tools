using Microsoft.AspNetCore.Mvc;
using System.IO;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Models;

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
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static TvWebApiResult Return(TvApiStatus status, string message, long? total = null, object? data = null) => new TvWebApiResult(status, message)
        {
            ContentType = "application/json; charset=utf-8",
            Total = total,
            Data = data
        };

        #region 返回成功
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static new TvWebApiResult Ok() => Return(TvApiStatus.Ok, "");

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult Ok(string message) => Return(TvApiStatus.Ok, message);

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Ok(string message, object data) => Return(TvApiStatus.Ok, message, null, data);

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Ok(string message, long total, object data) => Return(TvApiStatus.Ok, message, total, data);
        #endregion

        #region 返回失败
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <returns></returns>
        public static TvWebApiResult Fail() => Return(TvApiStatus.Fail, "");

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult Fail(string message) => Return(TvApiStatus.Fail, message);

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Fail(string message, object data) => Return(TvApiStatus.Fail, message, null, data);

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Fail(string message, long total, object data) => Return(TvApiStatus.Fail, message, total, data);
        #endregion

        #region 返回鉴权失败
        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <returns></returns>
        public static new TvWebApiResult Unauthorized() => Return(TvApiStatus.Unauthorized, "");

        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult Unauthorized(string message) => Return(TvApiStatus.Unauthorized, message);

        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Unauthorized(string message, object data) => Return(TvApiStatus.Unauthorized, message, null, data);

        /// <summary>
        /// 返回鉴权失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Unauthorized(string message, long total, object data) => Return(TvApiStatus.Unauthorized, message, total, data);
        #endregion

        #region 返回资源不存在
        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <returns></returns>
        public static new TvWebApiResult NotFound() => Return(TvApiStatus.NotFound, "");

        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult NotFound(string message) => Return(TvApiStatus.NotFound, message);

        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult NotFound(string message, object data) => Return(TvApiStatus.NotFound, message, null, data);

        /// <summary>
        /// 返回资源不存在
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult NotFound(string message, long total, object data) => Return(TvApiStatus.NotFound, message, total, data);
        #endregion

        #region 返回方法不被允许
        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <returns></returns>
        public static TvWebApiResult MethodNotAllowed() => Return(TvApiStatus.MethodNotAllowed, "");

        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult MethodNotAllowed(string message) => Return(TvApiStatus.MethodNotAllowed, message);

        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult MethodNotAllowed(string message, object data) => Return(TvApiStatus.MethodNotAllowed, message, null, data);

        /// <summary>
        /// 返回方法不被允许
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult MethodNotAllowed(string message, long total, object data) => Return(TvApiStatus.MethodNotAllowed, message, total, data);
        #endregion

        #region 返回异常
        /// <summary>
        /// 返回异常
        /// </summary>
        /// <returns></returns>
        public static TvWebApiResult Error() => Return(TvApiStatus.Error, "");

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TvWebApiResult Error(string message) => Return(TvApiStatus.Error, message);

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Error(string message, object data) => Return(TvApiStatus.Error, message, null, data);

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TvWebApiResult Error(string message, long total, object data) => Return(TvApiStatus.Error, message, total, data);
        #endregion

        #region 返回文件
        /// <summary>
        /// 返回文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static new TvWebApiStreamResult File(Stream stream, string fileName) => new TvWebApiStreamResult(stream, fileName)
        {
            ContentType = "application/octet-stream"
        };
        #endregion
    }
}