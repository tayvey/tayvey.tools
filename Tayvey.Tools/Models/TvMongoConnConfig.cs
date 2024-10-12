namespace Tayvey.Tools.Models
{
    /// <summary>
    /// MONGODB连接配置
    /// </summary>
    public sealed class TvMongoConnConfig
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnStr { get; set; } = "";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        public TvMongoConnConfig(string key)
        {
            Key = key;
        }
    }
}