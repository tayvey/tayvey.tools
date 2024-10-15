using System.Collections.Generic;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// SWAGGER配置
    /// </summary>
    public class TvSwaggerConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 抬头
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 程序集列表
        /// </summary>
        public List<string> AssemblyNames { get; set; } = new List<string>();

        /// <summary>
        /// 自定义请求头配置
        /// </summary>
        public List<Header> Headers { get; set; } = new List<Header>();

        /// <summary>
        /// 请求头配置
        /// </summary>
        public class Header
        {
            /// <summary>
            /// KEY
            /// </summary>
            public string Key { get; set; } = "";

            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; set; } = "";
        }
    }
}