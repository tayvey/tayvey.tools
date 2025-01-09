using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Tayvey.Tools
{
    /// <summary>
    /// 自动映射扩展
    /// </summary>
    public static class TvAutoMapper
    {
        /// <summary>
        /// 添加自动映射服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTvAutoMapper(this IServiceCollection services)
        {
            foreach (var mapper in GetAutoMappings())
            {
                services.AddAutoMapper(mapper);
            }
        }

        /// <summary>
        /// 获取自动映射的服务
        /// </summary>
        /// <returns></returns>
        private static List<Type> GetAutoMappings()
        {
            var result = new List<Type>();

            foreach (var loadedType in TvAssembly.GetLoadedAssemblies())
            {
                if (!loadedType.IsClass || loadedType.IsAbstract || !loadedType.IsSubclassOf(typeof(Profile)))
                {
                    continue;
                }

                result.Add(loadedType);
            }

            return result;
        }
    }
}