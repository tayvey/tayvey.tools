#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
using System;
using Microsoft.Extensions.Configuration;
#endif

using Tayvey.Tools.TvConfigs.Models;

namespace Tayvey.Tools.TvConfigs
{
    /// <summary>
    /// Tv配置
    /// </summary>
    public static class TvConfig
    {
        /// <summary>
        /// 配置对象
        /// </summary>
        private static IConfiguration? Configuration { get; set; }

        /// <summary>
        /// Tv配置
        /// </summary>
#if NET6_0_OR_GREATER
        internal static TvConfigOptions Options { get; private set; } = new();
#elif NETSTANDARD2_1
        internal static TvConfigOptions Options { get; private set; } = new TvConfigOptions();
#endif

        /// <summary>
        /// 初始化锁
        /// </summary>
#if NET6_0_OR_GREATER
        private static readonly object InitLock = new();
#elif NETSTANDARD2_1
        private static readonly object InitLock = new object();
#endif

        /// <summary>
        /// 初始化配置对象
        /// </summary>
        /// <param name="configuration">配置对象</param>
        public static void InitConfiguration(IConfiguration? configuration)
        {
            lock (InitLock)
            {
                if (Configuration == null && configuration != null)
                {
                    Configuration = configuration;
                    Options = Get<TvConfigOptions?>("TvConfig") ?? new TvConfigOptions();
                }
            }
        }

        /// <summary>
        /// 初始化配置对象
        /// </summary>
        /// <param name="files">配置文件列表</param>
        public static void InitConfiguration(params string[] files)
        {
            lock (InitLock)
            {
                if (Configuration == null && files.Length > 0)
                {
                    var configBuilder = new ConfigurationBuilder();

                    foreach (var file in files)
                    {
                        configBuilder.AddJsonFile(file);
                    }

                    Configuration = configBuilder.Build();
                    Options = Get<TvConfigOptions?>("TvConfig") ?? new TvConfigOptions();
                }
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">读取配置的KEY</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static T? Get<T>(string key)
#elif NETSTANDARD2_1
        public static T Get<T>(string key)
#endif
        {
            if (Configuration == null)
            {
                throw new Exception("配置对象未初始化");
            }

#if NETSTANDARD2_1
#pragma warning disable CS8603 // 可能返回 null 引用。
#endif
            return Configuration.GetSection(key).Get<T>();
#if NETSTANDARD2_1
#pragma warning restore CS8603 // 可能返回 null 引用。
#endif
        }
    }
}