using Microsoft.Extensions.DependencyInjection;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Models;
using Tayvey.Tools.Services;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// MONGODB扩展
    /// </summary>
    public static class TvMongoEx
    {
        /// <summary>
        /// 添加MONGODB服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvMongo(this IServiceCollection services, params TvMongoConnConfig[] configs)
        {
            services.AddSingleton<ITvMongo>(i => new TvMongo(configs));
            services.AddScoped(typeof(ITvMongoRepository<>), typeof(TvMongoRepository<>));
        }
    }
}