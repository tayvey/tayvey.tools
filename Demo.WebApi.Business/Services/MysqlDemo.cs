using Demo.WebApi.Business.Interfaces;
using Demo.WebApi.Model;
using SqlSugar;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Interfaces;

namespace Demo.WebApi.Business.Services;

/// <summary>
/// MYSQL DEMO业务
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class MysqlDemo(
    ITvSqlSugarRepository<MysqlDemoEntity> mysqlDemo) : IMysqlDemo
{
    /// <summary>
    /// MYSQL DEMO
    /// </summary>
    private readonly ITvSqlSugarRepository<MysqlDemoEntity> _mysqlDemo = mysqlDemo;

    /// <summary>
    /// 获取所有Name
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetNamesAsync()
    {
        var data = await _mysqlDemo.GetListAsync(i => true);
        return data.Select(i => i.Name).ToList();
    }
}