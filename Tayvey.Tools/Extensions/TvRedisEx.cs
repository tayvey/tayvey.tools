using Microsoft.Extensions.DependencyInjection;

namespace Tayvey.Tools
{
    /// <summary>
    /// Redis扩展
    /// </summary>
    public static class TvRedisEx
    {
        /// <summary>
        /// 添加Redis服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvRedis(this IServiceCollection services, params TvRedisConnectionConfig[] configs)
        {
            services.AddSingleton<ITvRedis>(i => new TvRedis(configs));
        }
    }
}