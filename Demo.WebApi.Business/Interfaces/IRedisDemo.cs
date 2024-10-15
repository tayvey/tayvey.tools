namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// REDIS DEMO业务接口
/// </summary>
public interface IRedisDemo
{
    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <returns></returns>
    public Task<string?> GetString();
}