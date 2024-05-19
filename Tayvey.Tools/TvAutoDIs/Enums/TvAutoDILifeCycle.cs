namespace Tayvey.Tools.TvAutoDIs.Enums
{
    /// <summary>
    /// Tv自动依赖注入生命周期
    /// </summary>
    public enum TvAutoDILifeCycle
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton,

        /// <summary>
        /// 瞬时
        /// </summary>
        Transient,

        /// <summary>
        /// 作用域
        /// </summary>
        Scoped
    }
}