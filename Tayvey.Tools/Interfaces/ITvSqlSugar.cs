using SqlSugar;

namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// SQLSUGAR接口
    /// </summary>
    public interface ITvSqlSugar
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetClient<T>() where T : class, new();
    }
}