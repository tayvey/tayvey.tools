using Demo.WebApi.Business.Interfaces;
using Demo.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Services;

namespace Demo.WebApi.Controller;

/// <summary>
/// DEMO
/// </summary>
[ApiController]
[Route("[Controller]/[Action]")]
public class DemoController : TvControllerBase
{
    private readonly IDemo _demo;
    private readonly IMongoDemo _mongoDemo;
    private readonly ITvConfig _config;
    private readonly IRedisDemo _redisDemo;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="demo"></param>
    public DemoController(IDemo demo, IMongoDemo mongoDemo, ITvConfig config, IRedisDemo redisDemo)
    {
        _demo = demo;
        _mongoDemo = mongoDemo;
        _config = config;
        _redisDemo = redisDemo;
    }

    /// <summary>
    /// 求和
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetSum(int a, int b)
    {
        return TvOk(null, _demo.Sum(a, b));
    }

    /// <summary>
    /// 获取ID列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetIdsAsync()
    {
        var data = await _mongoDemo.GetIdsAsync();
        return TvOk(null, data);
    }

    /// <summary>
    /// 获取名称列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetNamesAsync()
    {
        var data = await _mongoDemo.GetNamesAsync();
        return TvOk(null, data);
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetConfig()
    {
        return TvOk(null, _config.Get<string>("testConfig"));
    }

    /// <summary>
    /// 获取异常
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet]
    public IActionResult GetError()
    {
        throw new Exception("异常测试");
    }

    /// <summary>
    /// 获取REDIS字符串
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetRedisString()
    {
        var data = await _redisDemo.GetString();
        return TvOk(null, data);
    }

    /// <summary>
    /// 获取文件
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetFile()
    {
        var fs = await _demo.GenerateFileAsync();
        return TvFile(fs, "test.txt");
    }

    /// <summary>
    /// 模型校验
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult TestModelState(ModelStateDemoParam req)
    {
        return Ok();
    }
}