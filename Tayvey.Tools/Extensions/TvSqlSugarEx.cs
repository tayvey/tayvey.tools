using Microsoft.Extensions.DependencyInjection;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Models;
using Tayvey.Tools.Services;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// SQLSUGAR扩展
    /// </summary>
    public static class TvSqlSugarEx
    {
        /// <summary>
        /// 添加SQLSUGAR服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvSqlSugar(this IServiceCollection services, params TvSqlSugarConnConfig[] configs)
        {
            services.AddSingleton<ITvSqlSugar>(i => new TvSqlSugar(configs));
            services.AddScoped(typeof(ITvSqlSugarRepository<>), typeof(TvSqlSugarRepository<>));
        }
    }
}