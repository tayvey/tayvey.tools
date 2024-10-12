//#if NETSTANDARD2_1
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Tayvey.Tools.Service;
//using Tayvey.Tools.TvAutoDIs;
//using Tayvey.Tools.TvConfigs;
//using Tayvey.Tools.TvMiddlewares;
//using Tayvey.Tools.TvSoaps;
//using Tayvey.Tools.TvSwaggers;
//using Tayvey.Tools.TvTimedTasks;
//#endif

//using Tayvey.Tools.Service;
//using Tayvey.Tools.Extension;

//namespace Tayvey.Tools
//{
//    /// <summary>
//    /// Tayvey Tools
//    /// </summary>
//    public static class TvTools
//    {
//        /// <summary>
//        /// 添加Tayvey Tools
//        /// </summary>
//        /// <param name="service"></param>
//        /// <param name="configuration"></param>
//        /// <returns></returns>
//        public static IServiceCollection AddTvTools(this IServiceCollection service, IConfiguration configuration)
//        {
//            // 配置初始化
//            TvConfig.InitConfiguration(configuration);

//            // 依赖注入
//            service.AddTvAutoDI();

//            // SOAP
//            service.AddTvSoap();

//            // Swagger
//            service.AddTvSwagger();

//            // 定时任务
//            service.AddTvTimedTask();

//            // 模型校验
//            service.AddTvModelState();

//            return service;
//        }

//        /// <summary>
//        /// 使用Tayvey Tools
//        /// </summary>
//        /// <param name="app"></param>
//        /// <returns></returns>
//        public static IApplicationBuilder UseTvTools(this IApplicationBuilder app)
//        {
//            // 中间件
//            app.UseTvMiddleware();

//            // SOAP
//            app.UseTvSoap();

//            // Swagger
//            app.UseTvSwagger();

//            return app;
//        }
//    }
//}