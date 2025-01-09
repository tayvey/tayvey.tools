using SqlSugar;

namespace Tayvey.Tools
{
    /// <summary>
    /// 关系型数据库仓储接口
    /// </summary>
    public interface ITvSql
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ISqlSugarClient GetClient(string key);
    }
}