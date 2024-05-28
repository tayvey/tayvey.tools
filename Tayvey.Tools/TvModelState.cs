#if NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Tayvey.Tools.TvApiResults.Models;
#endif

namespace Tayvey.Tools
{
    /// <summary>
    /// Tv模型验证
    /// </summary>
    public static class TvModelState
    {
        /// <summary>
        /// 添加Tv模型验证
        /// </summary>
        /// <param name="service"></param>
        public static IServiceCollection AddTvModelState(this IServiceCollection service) => service.Configure<ApiBehaviorOptions>(opt =>
        {
            // 模型验证返回
            opt.InvalidModelStateResponseFactory = ac =>
            {
                // 获取错误消息
                var errorMsg = ac.ModelState
                    .AsParallel()
#if NET8_0_OR_GREATER
                    .Select(ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? [])
#else
                    .Select(ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>())
#endif
                    .SelectMany(ms => ms)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .First();

                // 异常消息转换
                errorMsg = errorMsg switch
                {
                    "The req field is required." => "参数异常",
                    _ when errorMsg.Contains("The JSON value could not be converted to") => "参数异常",
                    _ => errorMsg
                };

                // 返回
                return TvApiResult.Fail(errorMsg);
            };
        });
    }
}