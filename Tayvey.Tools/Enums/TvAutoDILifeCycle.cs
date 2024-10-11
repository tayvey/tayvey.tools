namespace Tayvey.Tools.Enums
{
    /// <summary>
    /// 自动依赖注入生命周期
    /// </summary>
    public enum TvAutoDILifeCycle
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// 作用域
        /// </summary>
        Scoped,

        /// <summary>
        /// 瞬时
        /// </summary>
        Transient
    }
}