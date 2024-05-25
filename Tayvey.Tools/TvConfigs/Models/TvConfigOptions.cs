#if NETSTANDARD2_1
using Tayvey.Tools.TvSwaggers.Models;
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
    }
}