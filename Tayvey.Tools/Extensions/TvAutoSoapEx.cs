using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoapCore;
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// SOAP扩展
    /// </summary>
    public static class TvAutoSoapEx
    {
        /// <summary>
        /// 添加SOAP服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTvSoap(this IServiceCollection services)
        {
            services.AddSoapCore();
        }

        /// <summary>
        /// 使用自动注册SOAP
        /// </summary>
        /// <param name="app"></param>
        /// <param name="marks"></param>
        /// <exception cref="Exception"></exception>
        public static void UseTvSoap(this IApplicationBuilder app, params string[] marks)
        {
            // 获取自动注册的SOAP
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(i => i.GetTypes())
                .SelectMany(i => i)
                .Where(i => i.IsClass && !i.IsAbstract && i.GetCustomAttribute<TvAutoSoapAttribute>() != null)
                .Select(i =>
                {
                    var attr = i.GetCustomAttribute<TvAutoSoapAttribute>()!;

                    if (attr._marks.Length == 0)
                    {
                        return new
                        {
                            Soap = i,
                            Version = attr._version,
                            Url = attr._url
                        };
                    }

                    if (!marks.Any(x => attr._marks.Contains(x)))
                    {
                        return null;
                    }

                    return new
                    {
                        Soap = i,
                        Version = attr._version,
                        Url = attr._url
                    };
                })
                .Where(i => i != null)
                .Select(i => i!)
                .ToList();

            // 检查是否存在重复URL
            var item = list.GroupBy(i => i.Url).FirstOrDefault(i => i.Count() > 1);
            if (item != null)
            {
                throw new Exception($"SOAP初始化异常. 存在重复URL. [{item.Key}]");
            }

            // 过滤无效URL
            list = list.Where(i => !string.IsNullOrWhiteSpace(i.Url)).ToList();

            // 遍历注册SOAP
            foreach (var config in list)
            {
                app.UseSoapEndpoint(config.Soap, config.Url, new SoapEncoderOptions
                {
                    MessageVersion = config.Version switch
                    {
                        TvAutoSoapVersion.Soap11 => MessageVersion.Soap11WSAddressingAugust2004,
                        _ => MessageVersion.Soap12WSAddressingAugust2004
                    }
                }, caseInsensitivePath: true);
            }
        }
    }
}