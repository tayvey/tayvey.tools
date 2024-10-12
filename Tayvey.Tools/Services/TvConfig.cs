using Microsoft.Extensions.Configuration;
using Tayvey.Tools.Interfaces;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class TvConfig : ITvConfig
    {
        /// <summary>
        /// 配置对象
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// WebApi初始化
        /// </summary>
        /// <param name="configuration"></param>
        public TvConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 自定义初始化
        /// </summary>
        /// <param name="files"></param>
        public TvConfig(params string[] files)
        {
            var configBuilder = new ConfigurationBuilder();

            foreach (var file in files)
            {
                configBuilder.AddJsonFile(file);
            }

            _configuration = configBuilder.Build();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">读取配置的KEY</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
#pragma warning disable CS8603 // 可能返回 null 引用。
            return _configuration.GetSection(key).Get<T>();
#pragma warning restore CS8603 // 可能返回 null 引用。
        }
    }
}