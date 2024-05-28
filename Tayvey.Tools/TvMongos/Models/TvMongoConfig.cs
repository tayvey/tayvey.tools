namespace Tayvey.Tools.TvMongos.Models
{
    /// <summary>
    /// MongoDB配置
    /// </summary>
    internal sealed class TvMongoConfig
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