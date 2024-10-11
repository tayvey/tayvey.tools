//#if NETSTANDARD2_1
//using Microsoft.Extensions.DependencyInjection;
//using Quartz;
//using Quartz.Impl;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Tayvey.Tools.TvConfigs;
//#endif

//namespace Tayvey.Tools.TvTimedTasks
//{
//    /// <summary>
//    /// Tv定时任务
//    /// </summary>
//    public static class TvTimedTask
//    {
//        /// <summary>
//        /// 调度器
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private static readonly Lazy<IScheduler> Scheduler = new(LzScheduler);
//#else
//        private static readonly Lazy<IScheduler> Scheduler = new Lazy<IScheduler>(LzScheduler);
//#endif

//        /// <summary>
//        /// 定时任务类
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private static readonly Lazy<List<Type>> Classes = new(LzJobs);
//#else
//        private static readonly Lazy<List<Type>> Classes = new Lazy<List<Type>>(LzJobs);
//#endif

//        /// <summary>
//        /// 懒加载调度器
//        /// </summary>
//        /// <returns></returns>
//        private static IScheduler LzScheduler()
//        {
//            // 创建调度器工厂
//            var factory = new StdSchedulerFactory();

//            // 获取调度器实例
//            var scheduler = factory.GetScheduler().Result;

//            // 开启调度器
//            scheduler.Start().Wait();

//            // 自动创建定时任务
//            AutoCreateTimedTasksAsync().Wait();

//            return scheduler;
//        }

//        /// <summary>
//        /// 初始化
//        /// </summary>
//        public static void Init()
//        {
//            _ = Scheduler.Value;
//        }

//        /// <summary>
//        /// 懒加载定时任务类
//        /// </summary>
//        /// <returns></returns>
//        private static List<Type> LzJobs() => TvRelyAssembly.LzRelyAssemblyTypes.Value
//            // 并行
//            .AsParallel()
//            // 筛选非抽象并且实现IJob接口的类
//            .Where(i => i.IsClass && !i.IsAbstract && i.GetInterface(nameof(IJob)) != null)
//            .ToList();

//        /// <summary>
//        /// 添加Tv定时任务
//        /// </summary>
//        /// <param name="service"></param>
//        public static IServiceCollection AddTvTimedTask(this IServiceCollection service)
//        {
//            // 自动添加计划任务
//            service.AddQuartz(q =>
//            {
//                // 遍历添加定时任务
//                foreach (var type in Classes.Value)
//                {
//                    // 获取配置
//                    var config = TvConfig.Options.TvTimedTask.FirstOrDefault(i => i.ClassName == type.Name);
//                    if (config == null || string.IsNullOrWhiteSpace(config.Cron))
//                    {
//                        continue;
//                    }

//                    // key
//                    var key = $"{type.Name}-{Guid.NewGuid():n}";
//                    var jobKey = new JobKey(key);

//                    // 创建作业
//                    q.AddJob(type, jobKey, j => j.WithIdentity(jobKey));

//                    // 创建触发器
//                    q.AddTrigger(j => j.ForJob(jobKey).WithIdentity($"{key}-trigger").WithCronSchedule(config.Cron));
//                }
//            });

//            // 将 Quartz.NET 调度器作为 ASP.NET Core 的后台服务 
//            // 应用程序关闭时，调度器会等待所有正在执行的作业完成后再停止
//            service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

//            return service;
//        }

//        /// <summary>
//        /// 自动创建定时任务
//        /// </summary>
//        /// <returns></returns>
//        private static async Task AutoCreateTimedTasksAsync()
//        {
//            // 遍历添加定时任务
//            foreach (var type in Classes.Value)
//            {
//                // 获取配置
//                var config = TvConfig.Options.TvTimedTask.FirstOrDefault(i => i.ClassName == type.Name);
//                if (config == null || string.IsNullOrWhiteSpace(config.Cron))
//                {
//                    continue;
//                }

//                // key
//                var key = $"{type.Name}-{Guid.NewGuid():n}";
//                var jobKey = new JobKey(key);

//                // 定义任务
//                var job = JobBuilder.Create(type).WithIdentity(jobKey).Build();

//                // 定义触发器，使用Cron表达式
//                var trigger = TriggerBuilder.Create().WithIdentity($"{key}-trigger").WithCronSchedule(config.Cron).Build();

//                // 调度任务
//                await Scheduler.Value.ScheduleJob(job, trigger);
//            }

//            // 监听控制台的关闭事件
//            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
//        }

//        /// <summary>
//        /// 监听控制台的关闭事件
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="args"></param>
//        private static void OnExit(object? sender, ConsoleCancelEventArgs args)
//        {
//            // 关闭调度器，等待所有作业完成后再停止
//            Scheduler.Value.Shutdown(true).Wait();

//            // 结束应用程序
//            Environment.Exit(0);
//        }
//    }
//}