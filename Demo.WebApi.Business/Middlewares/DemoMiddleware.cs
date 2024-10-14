using Microsoft.AspNetCore.Http;
using Tayvey.Tools.Attributes;

namespace Demo.WebApi.Business.Middlewares;

/// <summary>
/// DEMO中间件
/// </summary>
[TvAutoMw]
public class DemoMiddleware
{
    /// <summary>
    /// 委托
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="next"></param>
    public DemoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        await _next(context);
    }
}