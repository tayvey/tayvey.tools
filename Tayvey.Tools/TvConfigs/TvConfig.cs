//#if NETSTANDARD2_1
//using Microsoft.Extensions.Configuration;
//using System;
//using Tayvey.Tools.TvConfigs.Models;
//#endif

//namespace Tayvey.Tools.TvConfigs
//{
//    /// <summary>
//    /// Tv配置
//    /// </summary>
//    public static class TvConfig
//    {
//        /// <summary>
//        /// 配置对象
//        /// </summary>
//        private static IConfiguration? Configuration { get; set; }

//        /// <summary>
//        /// Tv配置
//        /// </summary>
//#if NET6_0_OR_GREATER
//        internal static TvConfigOptions Options { get; private set; } = new();
//#else
//        internal static TvConfigOptions Options { get; private set; } = new TvConfigOptions();
//#endif

//        /// <summary>
//        /// 初始化锁
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private static readonly object InitLock = new();
//#else
//        private static readonly object InitLock = new object();
//#endif

//        /// <summary>
//        /// 初始化配置对象
//        /// </summary>
//        /// <param name="configuration">配置对象</param>
//        public static void InitConfiguration(IConfiguration? configuration)
//        {
//            lock (InitLock)
//            {
//                if (Configuration == null && configuration != null)
//                {
//                    Configuration = configuration;
//                    Options = Get<TvConfigOptions?>("TvConfig") ?? new TvConfigOptions();
//                }
//            }
//        }

//        /// <summary>
//        /// 初始化配置对象
//        /// </summary>
//        /// <param name="files">配置文件列表</param>
//        public static void InitConfiguration(params string[] files)
//        {
//            lock (InitLock)
//            {
//                if (Configuration == null && files.Length > 0)
//                {
//                    var configBuilder = new ConfigurationBuilder();

//                    foreach (var file in files)
//                    {
//                        configBuilder.AddJsonFile(file);
//                    }

//                    Configuration = configBuilder.Build();
//                    Options = Get<TvConfigOptions?>("TvConfig") ?? new TvConfigOptions();
//                }
//            }
//        }

//        /// <summary>
//        /// 获取配置
//        /// </summary>
//        /// <typeparam name="T">返回类型</typeparam>
//        /// <param name="key">读取配置的KEY</param>
//        /// <returns></returns>
//#if NET6_0_OR_GREATER
//        public static T? Get<T>(string key)
//#else
//        public static T Get<T>(string key)
//#endif
//        {
//            if (Configuration == null)
//            {
//                throw new Exception("配置对象未初始化");
//            }

//#if NET6_0_OR_GREATER
//            return Configuration.GetSection(key).Get<T>();
//#else
//#pragma warning disable CS8603 // 可能返回 null 引用。
//            return Configuration.GetSection(key).Get<T>();
//#pragma warning restore CS8603 // 可能返回 null 引用。
//#endif
//        }
//    }
//}