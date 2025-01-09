namespace Tayvey.Tools
{
    /// <summary>
    /// Redis连接配置
    /// </summary>
    public sealed class TvRedisConnectionConfig
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; } = "";
    }
}