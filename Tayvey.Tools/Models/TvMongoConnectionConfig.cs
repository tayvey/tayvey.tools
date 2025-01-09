namespace Tayvey.Tools
{
    /// <summary>
    /// MongoDB连接配置
    /// </summary>
    public sealed class TvMongoConnectionConfig
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