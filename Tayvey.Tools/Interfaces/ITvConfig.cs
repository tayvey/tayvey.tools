namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// 系统配置接口
    /// </summary>
    public interface ITvConfig
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">读取配置的KEY</param>
        /// <returns></returns>
        T Get<T>(string key);
    }
}