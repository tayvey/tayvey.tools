using Microsoft.Extensions.DependencyInjection;

namespace Tayvey.Tools
{
    /// <summary>
    /// MongoDB扩展
    /// </summary>
    public static class TvMongoEx
    {
        /// <summary>
        /// 添加MongoDB服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvMongo(this IServiceCollection services, params TvMongoConnectionConfig[] configs)
        {
            services.AddSingleton<ITvMongo>(i => new TvMongo(configs));
        }
    }
}