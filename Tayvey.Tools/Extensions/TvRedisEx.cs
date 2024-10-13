using Microsoft.Extensions.DependencyInjection;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Models;
using Tayvey.Tools.Services;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// REDIS扩展
    /// </summary>
    public static class TvRedisEx
    {
        /// <summary>
        /// 添加REDIS服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvRedis(this IServiceCollection services, params TvRedisConnConfig[] configs)
        {
            services.AddSingleton<ITvRedis>(i => new TvRedis(configs));
        }
    }
}