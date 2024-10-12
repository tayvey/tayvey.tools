﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tayvey.Tools.Interface;
using Tayvey.Tools.Service;

namespace Tayvey.Tools.Extension
{
    /// <summary>
    /// 系统配置扩展
    /// </summary>
    public static class TvConfigEx
    {
        /// <summary>
        /// 添加系统配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddTvConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITvConfig>(p => new TvConfig(configuration));
        }
    }
}