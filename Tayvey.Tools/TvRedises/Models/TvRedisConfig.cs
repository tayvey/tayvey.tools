namespace Tayvey.Tools.TvRedises.Models
{
    /// <summary>
    /// TvRedis配置
    /// </summary>
    internal sealed class TvRedisConfig
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; } = "";
    }
}