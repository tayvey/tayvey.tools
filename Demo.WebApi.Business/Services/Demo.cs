using Demo.WebApi.Business.Interfaces;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Demo.WebApi.Business.Services;

/// <summary>
/// 业务逻辑DEMO
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class Demo : IDemo
{
    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int Sum(int a, int b)
    {
        return a + b;
    }
}