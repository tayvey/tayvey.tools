namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// MSSQL DEMO业务接口
/// </summary>
public interface IMssqlDemo
{
    /// <summary>
    /// 获取所有Name
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetNamesAsync();
}