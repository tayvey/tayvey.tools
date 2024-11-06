namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// MYSQL DEMO业务接口
/// </summary>
public interface IMysqlDemo
{
    /// <summary>
    /// 获取所有Name
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetNamesAsync();
}