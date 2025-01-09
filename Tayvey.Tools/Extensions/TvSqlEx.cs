using Microsoft.Extensions.DependencyInjection;

namespace Tayvey.Tools
{
    /// <summary>
    /// 关系型数据库扩展
    /// </summary>
    public static class TvSqlEx
    {
        /// <summary>
        /// 添加关系型数据库服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        public static void AddTvSql(this IServiceCollection services, params TvSqlConnectionConfig[] configs)
        {
            services.AddSingleton<ITvSql>(i => new TvSql(configs));
        }
    }
}