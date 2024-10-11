//#if NETSTANDARD2_1
//using System.Collections.Generic;
//#endif

//namespace Tayvey.Tools.TvSwaggers.Models
//{
//    /// <summary>
//    /// TvSwagger配置
//    /// </summary>
//    internal sealed class TvSwaggerConfig
//    {
//        /// <summary>
//        /// 名称
//        /// </summary>
//        public string Name { get; set; } = "swagger";

//        /// <summary>
//        /// 版本
//        /// </summary>
//        public string Version { get; set; } = "1.0";

//        /// <summary>
//        /// 请求头配置
//        /// </summary>
//#if NET8_0_OR_GREATER
//        public List<TvSwaggerHeaderConfig> Headers { get; set; } = [];
//#elif NET6_0
//        public List<TvSwaggerHeaderConfig> Headers { get; set; } = new();
//#else
//        public List<TvSwaggerHeaderConfig> Headers { get; set; } = new List<TvSwaggerHeaderConfig>();
//#endif

//        /// <summary>
//        /// 请求头配置
//        /// </summary>
//        internal class TvSwaggerHeaderConfig
//        {
//            /// <summary>
//            /// 请求头Key
//            /// </summary>
//            public string Key { get; set; } = "";

//            /// <summary>
//            /// 请求头说明
//            /// </summary>
//            public string Desc { get; set; } = "...";
//        }
//    }
//}