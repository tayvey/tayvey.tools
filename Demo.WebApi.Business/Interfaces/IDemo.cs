namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// DEMO接口
/// </summary>
public interface IDemo
{
    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int Sum(int a, int b);
}