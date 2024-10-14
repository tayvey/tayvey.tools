using Demo.WebApi.Business.Interfaces;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Demo.WebApi.Business.Services;

/// <summary>
/// SOAP DEMO
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class SoapDemo : ISoapDemo
{
    /// <summary>
    /// DEMO业务
    /// </summary>
    private readonly IDemo _demo;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="demo"></param>
    public SoapDemo(IDemo demo)
    {
        _demo = demo;
    }

    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int GetSum(int a, int b)
    {
        return _demo.Sum(a, b);
    }
}