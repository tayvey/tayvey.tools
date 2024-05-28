#if NETSTANDARD2_1
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.TvAutoDIs.Attrs;
using Tayvey.Tools.TvAutoDIs.Enums;
using Tayvey.Tools.TvAutoDIs.Models;
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
#if NET6_0_OR_GREATER        
        private static readonly Lazy<ServiceProvider> Service = new(LzService);
#else
        private static readonly Lazy<ServiceProvider> Service = new Lazy<ServiceProvider>(LzService);
#endif

        /// <summary>
        /// 自动依赖注入的类
        /// </summary>
#if NET6_0_OR_GREATER        
        private static readonly Lazy<List<TvAutoDIClass>> Classes = new(LzClasses);
#else
        private static readonly Lazy<List<TvAutoDIClass>> Classes = new Lazy<List<TvAutoDIClass>>(LzClasses);
#endif

        /// <summary>
        /// 懒加载服务容器
        /// </summary>
        /// <returns></returns>
        private static ServiceProvider LzService()
        {
            // 创建服务集合
            var services = new ServiceCollection();

            // 自动依赖注入
            services.AddTvAutoDI();

            // 构建服务提供程序
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 懒加载可以自动依赖注入的类
        /// </summary>
        /// <returns></returns>
        private static List<TvAutoDIClass> LzClasses()
        {
            // 获取所有类
            var typePq = TvRelyAssembly.LzRelyAssemblyTypes.Value
                // 并行处理
                .AsParallel()
                // 筛选非抽象类
                .Where(type => type.IsClass && !type.IsAbstract)
                // 筛选有TvAutoDI特性的类
                .Where(type => type.GetCustomAttribute<TvAutoDIAttribute>() != null);

            // 获取类直接实现的所有接口
            return typePq.Select(type =>
            {
                // 类实现的所有接口
                var allInterfaces = type.GetInterfaces();

                // 父接口实现的所有接口
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(i => i.GetInterfaces());

                // 排除非直接实现的接口
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();

                // TvAutoDI特性
                var attr = type.GetCustomAttribute<TvAutoDIAttribute>()!;

                return new TvAutoDIClass(type, attr)
                {
                    Interfaces = interfaces
                };
            }).ToList();
        }

        /// <summary>
        /// 添加Tv自动依赖注入
        /// </summary>
        /// <param name="service"></param>
        public static void AddTvAutoDI(this IServiceCollection service)
        {
            // 遍历依赖注入
            foreach (var item in Classes.Value)
            {
                // 无接口依赖注入
                if (item.Interfaces.Count == 0 || item.Attr.IgnoreInterface)
                {
                    _ = item.Attr.LifeCycle switch
                    {
                        TvAutoDILifeCycle.Singleton => service.AddSingleton(item.Clz),
                        TvAutoDILifeCycle.Scoped => service.AddScoped(item.Clz),
                        TvAutoDILifeCycle.Transient => service.AddTransient(item.Clz),
                        _ => service
                    };

                    continue;
                }

                // 遍历接口依赖注入
                foreach (var i in item.Interfaces)
                {
                    _ = item.Attr.LifeCycle switch
                    {
                        TvAutoDILifeCycle.Singleton => service.AddSingleton(i, item.Clz),
                        TvAutoDILifeCycle.Scoped => service.AddScoped(i, item.Clz),
                        TvAutoDILifeCycle.Transient => service.AddTransient(i, item.Clz),
                        _ => service
                    };
                }
            }
        }
        
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : notnull
        {
            return Service.Value.GetRequiredService<T>();
        }
    }
}