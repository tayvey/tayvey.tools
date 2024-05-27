#if NETSTANDARD2_1
using Tayvey.Tools.TvSwaggers.Models;
using Tayvey.Tools.TvMongos.Models;
using System.Collections.Generic;
using Tayvey.Tools.TvRedises.Models;
#endif

namespace Tayvey.Tools.TvConfigs.Models
{
    /// <summary>
    /// Tv配置项
    /// </summary>
    internal sealed class TvConfigOptions
    {
        /// <summary>
        /// TvSwagger配置
        /// </summary>
#if NET6_0_OR_GREATER
        public TvSwaggerConfig TvSwagger { get; set; } = new();
#else
        public TvSwaggerConfig TvSwagger { get; set; } = new TvSwaggerConfig();
#endif

        /// <summary>
        /// TvMongoDB配置
        /// </summary>
#if NET8_0_OR_GREATER
        public List<TvMongoConfig> TvMongo { get; set; } = [];
#elif NET6_0_OR_GREATER
        public List<TvMongoConfig> TvMongo { get; set; } = new();
#else
        public List<TvMongoConfig> TvMongo { get; set; } = new List<TvMongoConfig>();
#endif

        /// <summary>
        /// TvRedis配置
        /// </summary>
#if NET8_0_OR_GREATER
        public List<TvRedisConfig> TvRedis { get; set; } = [];
#elif NET6_0_OR_GREATER
        public List<TvRedisConfig> TvRedis { get; set; } = new();
#else
        public List<TvRedisConfig> TvRedis { get; set; } = new List<TvRedisConfig>();
#endif
    }
}