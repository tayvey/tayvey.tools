using Tayvey.Tools.Attributes;
using SqlSugar;

namespace Demo.WebApi.Model;

/// <summary>
/// MSSQL DEMO实体
/// </summary>
[TvSqlSugar("sqlServer")]
[SugarTable("user")]
public class MssqlDemoEntity
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