using Tayvey.Tools.Attributes;
using SqlSugar;

namespace Demo.WebApi.Model;

/// <summary>
/// MYSQL DEMO实体
/// </summary>
[TvSqlSugar("mysql")]
[SugarTable("user")]
public class MysqlDemoEntity
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = "";
}