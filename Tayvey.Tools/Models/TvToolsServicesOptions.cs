using System;

namespace Tayvey.Tools
{
    /// <summary>
    /// TV工具服务注册选项
    /// </summary>
    public sealed class TvToolsServicesOptions
    {
        /// <summary>
        /// 是否启用SOAP
        /// </summary>
        public bool IsEnableSOAP { get; set; } = true;

        /// <summary>
        /// mongodb连接配置
        /// </summary>
        public TvMongoConnectionConfig[] MongoConnectionConfigs { get; set; } = Array.Empty<TvMongoConnectionConfig>();

        /// <summary>
        /// 关系型数据库连接配置
        /// </summary>
        public TvSqlConnectionConfig[] SqlConnectionConfigs { get; set; } = Array.Empty<TvSqlConnectionConfig>();

        /// <summary>
        /// redis连接配置
        /// </summary>
        public TvRedisConnectionConfig[] RedisConnectionConfigs { get; set; } = Array.Empty<TvRedisConnectionConfig>();

        /// <summary>
        /// swagger配置
        /// </summary>
        public TvSwaggerConfig? SwaggerConfig { get; set; } = null;

        /// <summary>
        /// 是否启用定时任务
        /// </summary>
        public bool IsEnableCron { get; set; } = true;

        /// <summary>
        /// 定时任务启动标记
        /// </summary>
        public string[] CronJobMarks { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 是否启用模型校验
        /// </summary>
        public bool IsEnableModelState { get; set; } = true;

        /// <summary>
        /// 是否启用依赖注入
        /// </summary>
        public bool IsEnableDI { get; set; } = true;

        /// <summary>
        /// 是否启用对象映射
        /// </summary>
        public bool IsEnableMapper { get; set; } = true;
    }
}