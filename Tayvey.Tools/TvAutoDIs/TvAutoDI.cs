#if NETSTANDARD2_1
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.TvAutoDIs.Attrs;
using Tayvey.Tools.TvAutoDIs.Enums;
#endif

namespace Tayvey.Tools.TvAutoDIs
{
    /// <summary>
    /// Tv自动依赖注入
    /// </summary>
    public static class TvAutoDI
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        public static ServiceProvider? Service { get; private set; }

        /// <summary>
        /// 初始化锁
        /// </summary>
#if NET6_0_OR_GREATER
        private static readonly object InitLock = new();
#else
        private static readonly object InitLock = new object();
#endif

        /// <summary>
        /// 添加Tv自动依赖注入
        /// </summary>
        /// <param name="service"></param>
        public static void AddTvAutoDI(this IServiceCollection service)
        {
            // 获取程序集列表
            var assembliesPq = GetAssemblies().AsParallel();

            // 获取自动依赖注入的类
            var classPq = assembliesPq.Select(GetAutoDIClass).SelectMany(item => item);

            var test = classPq.ToList();

            // 遍历依赖注入
            foreach (var (clz, interfaces, attr) in classPq)
            {
                // 生命周期
                var lifeCycle = attr.LifeCycle;

                // 忽略接口
                var ignoreInterface = attr.IgnoreInterface;

                // 自动依赖注入
                AutoDI(service, clz, interfaces, lifeCycle, ignoreInterface);
            }
        }

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <returns></returns>
        internal static List<Assembly> GetAssemblies()
        {
            // 获取入口程序集
            var entry = Assembly.GetEntryAssembly();

            // 获取Tayvey.Tools程序集
            var tvDll = Assembly.GetExecutingAssembly();

            // 获取入口程序集引用的程序集
            var rAssembliesPq = entry!.GetReferencedAssemblies().AsParallel();

            // 获取已加载的程序集
            var loadAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            // 筛选非Tayvey.Tools程序集
            rAssembliesPq = rAssembliesPq.Where(i => i.FullName != tvDll.FullName);

            // 筛选未加载的程序集
            rAssembliesPq = rAssembliesPq.Where(i => !loadAssemblies.AsParallel().Any(x => x.FullName == i.FullName));

            // 加载程序集
            var assembliesPq = rAssembliesPq.Select(Assembly.Load).AsParallel();
            loadAssemblies.AddRange(assembliesPq);

            // 获取引用了Tayvey.Tools的程序集
            assembliesPq = loadAssemblies.AsParallel().Where(ab =>
            {
                var raPq = ab.GetReferencedAssemblies().AsParallel();
                return raPq.Any(ra => ra.FullName == tvDll.FullName);
            });

            return assembliesPq.ToList();
        }

        /// <summary>
        /// 获取自动依赖注入的类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        private static ParallelQuery<(Type clz, List<Type> interfaces, TvAutoDIAttribute attr)> GetAutoDIClass(Assembly assembly)
        {
            // 获取所有类
            var typePq = assembly.GetTypes().AsParallel();

            // 筛选非抽象类
            typePq = typePq.Where(type => type.IsClass && !type.IsAbstract);

            // 筛选有TvAutoDI特性的类
            typePq = typePq.Where(type => type.GetCustomAttribute<TvAutoDIAttribute>() != null);

            // 获取类直接实现的所有接口
            var classPq = typePq.Select(type =>
            {
                // 类实现的所有接口
                var allInterfaces = type.GetInterfaces();

                // 父接口实现的所有接口
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(i => i.GetInterfaces());

                // 排除非直接实现的接口
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();

                return (type, interfaces, type.GetCustomAttribute<TvAutoDIAttribute>()!);
            }).AsParallel();

            return classPq;
        }

        /// <summary>
        /// 自动依赖注入
        /// </summary>
        /// <param name="service">服务注册容器</param>
        /// <param name="clz">类</param>
        /// <param name="interfaces">类直接实现的接口</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="ignoreInterface">是否忽略接口</param>
        private static void AutoDI(
            IServiceCollection service,
            Type clz,
            List<Type> interfaces,
            TvAutoDILifeCycle lifeCycle,
            bool ignoreInterface
        )
        {
            // 接口为空或者忽略接口
            if (interfaces.Count == 0 || ignoreInterface)
            {
                switch (lifeCycle)
                {
                    case TvAutoDILifeCycle.Singleton:
                        service.AddSingleton(clz);
                        break;
                    case TvAutoDILifeCycle.Scoped:
                        service.AddScoped(clz);
                        break;
                    case TvAutoDILifeCycle.Transient:
                        service.AddTransient(clz);
                        break;
                }
                return;
            }

            // 遍历依赖注入
            foreach (var item in interfaces)
            {
                switch (lifeCycle)
                {
                    case TvAutoDILifeCycle.Singleton:
                        service.AddSingleton(item, clz);
                        break;
                    case TvAutoDILifeCycle.Scoped:
                        service.AddScoped(item, clz);
                        break;
                    case TvAutoDILifeCycle.Transient:
                        service.AddTransient(item, clz);
                        break;
                }
            }
        }

        /// <summary>
        /// 初始化服务容器
        /// </summary>
        public static void Init()
        {
            lock (InitLock)
            {
                if (Service == null)
                {
                    // 创建服务集合
                    var services = new ServiceCollection();

                    // 自动依赖注入
                    services.AddTvAutoDI();

                    // 构建服务提供程序
                    Service = services.BuildServiceProvider();
                }
            }
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        public static T? Get<T>() where T : notnull
#else
        public static T Get<T>() where T : notnull
#endif
        {
            if (Service == null)
            {
#if NET6_0_OR_GREATER
                return default;
#else
#pragma warning disable CS8603 // 可能返回 null 引用。
                return default;
#pragma warning restore CS8603 // 可能返回 null 引用。
#endif
            }

            return Service.GetRequiredService<T>();
        }
    }
}