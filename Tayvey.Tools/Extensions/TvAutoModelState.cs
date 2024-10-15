using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Extensions
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
        /// <param name="failProcess"></param>
        public static void AddTvAutoModelState(this IServiceCollection services, Func<ActionContext, IActionResult>? failProcess = null) => services.Configure<ApiBehaviorOptions>(opt =>
        {
            if (failProcess != null)
            {
                opt.InvalidModelStateResponseFactory = failProcess;
            }
            else
            {
                opt.InvalidModelStateResponseFactory = ac =>
                {
                    // 获取错误消息
                    var errorMsg = ac.ModelState
                        .AsParallel()
                        .Select(ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>())
                        .SelectMany(ms => ms)
                        .Where(i => !string.IsNullOrWhiteSpace(i))
                        .First();

                    return new TvWebApiResult(TvApiStatus.Fail)
                    {
                        ContentType = "application/json; charset=utf-8",
                        Message = errorMsg
                    };
                };
            }
        });
    }
}