using Demo.WebApi.Business.Interfaces;
using StackExchange.Redis;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Interfaces;

namespace Demo.WebApi.Business.Services;

/// <summary>
/// REDIS DEMO业务
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class RedisDemo : IRedisDemo
{
    /// <summary>
    /// REDIS
    /// </summary>
    private readonly IDatabase _redis;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="redis"></param>
    public RedisDemo(ITvRedis redis)
    {
        _redis = redis.GetDatabase("testKey");
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetString()
    {
        return await _redis.StringGetAsync("test");
    }
}