using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Tayvey.Tools
{
    /// <summary>
    /// 自动模型校验
    /// </summary>
    public static class TvAutoModelState
    {
        /// <summary>
        /// 添加自动模型校验服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTvAutoModelState(this IServiceCollection services) => services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = ac =>
            {
                // 获取错误消息
                var errorMsgHs = ac.ModelState
                    .AsParallel()
                    .Select(ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>())
                    .SelectMany(ms => ms)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToHashSet();

                return new ObjectResult(new
                {
                    StatusCode = 400,
                    Message = "请求失败",
                    Data = errorMsgHs
                })
                {
                    StatusCode = 400
                };
            };
        });
    }
}