namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// MONGODB DEMO业务接口
/// </summary>
public interface IMongoDemo
{
    /// <summary>
    /// 获取ID列表
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetIdsAsync();

    /// <summary>
    /// 获取名称列表
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetNamesAsync();
}