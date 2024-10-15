using Demo.WebApi.Business.Interfaces;
using System.Text;
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

    /// <summary>
    /// 生成文件
    /// </summary>
    /// <returns></returns>
    public async Task<FileStream> GenerateFileAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "test.txt");
        var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        var buffer = Encoding.UTF8.GetBytes("hello");
        await fs.WriteAsync(buffer, 0, buffer.Length);

        return fs;
    }
}