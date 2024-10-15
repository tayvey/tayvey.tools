using Quartz;
using Tayvey.Tools.Attributes;

namespace Demo.WebApi.Business.Jobs;

/// <summary>
/// 定时任务DEMO
/// </summary>
[TvAutoCronJob("0/10 * * * * ? *")]
public class JobDemo : IJob
{
    /// <summary>
    /// 执行定时任务
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}定时任务已执行");
        return Task.CompletedTask;
    }
}