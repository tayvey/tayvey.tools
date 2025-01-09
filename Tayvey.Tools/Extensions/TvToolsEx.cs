using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using SoapCore;

namespace Tayvey.Tools
{
    /// <summary>
    /// tayvey工具扩展
    /// </summary>
    public static class TvToolsEx
    {
        /// <summary>
        /// 添加tayvey工具服务
        /// </summary>
        /// <param name="host"></param>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void AddTvTools(this IHostBuilder host, IServiceCollection services, Action<TvToolsServicesOptions>? options = null)
        {
            var _options = new TvToolsServicesOptions();
            options?.Invoke(_options);

            if (WindowsServiceHelpers.IsWindowsService())
            {
                host.UseContentRoot(AppContext.BaseDirectory).UseWindowsService();
            }

            if (_options.IsEnableSOAP)
            {
                services.AddSoapCore();
            }

            if (_options.MongoConnectionConfigs.Length > 0)
            {
                services.AddTvMongo(_options.MongoConnectionConfigs);
            }

            if (_options.SqlConnectionConfigs.Length > 0)
            {
                services.AddTvSql(_options.SqlConnectionConfigs);
            }

            if (_options.RedisConnectionConfigs.Length > 0)
            {
                services.AddTvRedis(_options.RedisConnectionConfigs);
            }

            if (_options.SwaggerConfig != null)
            {
                services.AddTvSwagger(_options.SwaggerConfig);
            }

            if (_options.IsEnableCron)
            {
                services.AddTvAutoCronJob(_options.CronJobMarks);
            }

            if (_options.IsEnableModelState)
            {
                services.AddTvAutoModelState();
            }

            if (_options.IsEnableDI)
            {
                services.AddTvAutoDI();
            }

            if (_options.IsEnableMapper)
            {
                services.AddTvAutoMapper();
            }
        }

        /// <summary>
        /// 使用tayvey工具
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseTvTools(this IApplicationBuilder app, Action<TvToolsAppOptions>? options = null)
        {
            var _options = new TvToolsAppOptions();
            options?.Invoke(_options);

            if (_options.IsEnableGlobalException)
            {
                app.UseTvGlobalEx();
            }

            if (_options.IsEnableSOAP)
            {
                app.UseTvAutoSoap(_options.SOAPMarks);
            }

            if (_options.IsEnableSwagger)
            {
                app.UseTvSwagger();
            }

            if (_options.IsEnableMiddleware)
            {
                app.UseTvAutoMw(_options.MiddlewareMarks);
            }
        }
    }
}