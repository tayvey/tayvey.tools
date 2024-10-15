using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Helpers;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// 自动注册定时任务扩展
    /// </summary>
    public static class TvAutoCronJobEx
    {
        /// <summary>
        /// 添加自动定时任务服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="marks"></param>
        public static void AddTvAutoCronJob(this IServiceCollection services, params string[] marks)
        {
            services.AddQuartz(q =>
            {
                // 遍历添加定时任务
                foreach (var (cronJob, cron) in GetAutoCronJobs(marks))
                {
                    // key
                    var key = $"{cronJob.Name}-{Guid.NewGuid():n}";
                    var jobKey = new JobKey(key);

                    // 创建作业
                    q.AddJob(cronJob, jobKey, j => j.WithIdentity(jobKey));

                    // 创建触发器
                    q.AddTrigger(j => j.ForJob(jobKey).WithIdentity($"{key}-trigger").WithCronSchedule(cron));
                }
            });

            // 将 Quartz.NET 调度器作为 ASP.NET Core 的后台服务 
            // 应用程序关闭时，调度器会等待所有正在执行的作业完成后再停止
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        /// <summary>
        /// 获取自动注册的定时任务
        /// </summary>
        /// <param name="marks"></param>
        /// <returns></returns>
        private static List<(Type cronJob, string cron)> GetAutoCronJobs(string[] marks)
        {
            var result = new List<(Type, string)>();

            foreach (var loadedType in TvAssembly.GetLoadedAssemblies())
            {
                var attr = loadedType.GetCustomAttribute<TvAutoCronJobAttribute>();
                if (!loadedType.IsClass || loadedType.IsAbstract || attr == null)
                {
                    continue;
                }

                if (attr._marks.Length > 0 && !marks.Any(x => attr._marks.Contains(x)))
                {
                    continue;
                }

                var allInterfaces = loadedType.GetInterfaces();
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(x => x.GetInterfaces());
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();
                if (!interfaces.Any(i => i.FullName == "Quartz.IJob"))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(attr._cron))
                {
                    continue;
                }

                result.Add((loadedType, attr._cron));
            }

            return result;
        }
    }
}