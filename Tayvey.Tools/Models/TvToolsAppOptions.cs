using System;

namespace Tayvey.Tools
{
    /// <summary>
    /// TV工具请求管道配置选项
    /// </summary>
    public sealed class TvToolsAppOptions
    {
        /// <summary>
        /// 是否启用全局异常捕获
        /// </summary>
        public bool IsEnableGlobalException { get; set; } = true;

        /// <summary>
        /// 是否启用SOAP
        /// </summary>
        public bool IsEnableSOAP { get; set; } = true;

        /// <summary>
        /// SOAP启动标记
        /// </summary>
        public string[] SOAPMarks { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 是否启用swagger
        /// </summary>
        public bool IsEnableSwagger { get; set; } = true;

        /// <summary>
        /// 是否启用中间件
        /// </summary>
        public bool IsEnableMiddleware { get; set; } = true;

        /// <summary>
        /// 中间件启动标记
        /// </summary>
        public string[] MiddlewareMarks { get; set; } = Array.Empty<string>();
    }
}