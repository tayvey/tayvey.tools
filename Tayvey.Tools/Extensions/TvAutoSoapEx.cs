using Microsoft.AspNetCore.Builder;
using SoapCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Helpers;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// SOAP扩展
    /// </summary>
    public static class TvAutoSoapEx
    {
        /// <summary>
        /// 使用自动注册SOAP
        /// </summary>
        /// <param name="app"></param>
        /// <param name="marks"></param>
        /// <exception cref="Exception"></exception>
        public static void UseTvAutoSoap(this IApplicationBuilder app, params string[] marks)
        {
            // 获取自动注册的SOAP
            var autoSoaps = GetAutoSoapInterfaces(marks);
            autoSoaps.AddRange(GetAutoSoapClasses(marks));

            // 检查是否存在重复URL
            var autoSoap = autoSoaps.GroupBy(i => i.url).FirstOrDefault(i => i.Count() > 1);
            if (autoSoap != null)
            {
                throw new Exception($"SOAP初始化异常. 存在重复URL. [{autoSoap.Key}]");
            }

            // 遍历注册SOAP
            foreach (var (curAutoSoap, version, url) in autoSoaps)
            {
                app.UseSoapEndpoint(curAutoSoap, url, new SoapEncoderOptions
                {
                    MessageVersion = version switch
                    {
                        TvAutoSoapVersion.Soap11 => MessageVersion.Soap11WSAddressingAugust2004,
                        _ => MessageVersion.Soap12WSAddressingAugust2004
                    }
                }, caseInsensitivePath: true);
            }
        }

        /// <summary>
        /// 获取自动注册的SOAP接口
        /// </summary>
        /// <param name="marks"></param>
        /// <returns></returns>
        private static List<(Type autoSoap, TvAutoSoapVersion version, string url)> GetAutoSoapInterfaces(string[] marks)
        {
            var result = new List<(Type, TvAutoSoapVersion, string)>();

            foreach (var loadedType in TvAssembly.GetLoadedAssemblies())
            {
                var autoSoapAttr = loadedType.GetCustomAttribute<TvAutoSoapAttribute>();
                var serviceContractAttr = loadedType.GetCustomAttribute<ServiceContractAttribute>();
                if (!loadedType.IsInterface || autoSoapAttr == null || serviceContractAttr == null)
                {
                    continue;
                }

                if (autoSoapAttr._marks.Length > 0 && !marks.Any(x => autoSoapAttr._marks.Contains(x)))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(autoSoapAttr._url))
                {
                    continue;
                }

                result.Add((loadedType, autoSoapAttr._version, autoSoapAttr._url));
            }

            return result;
        }

        /// <summary>
        /// 获取自动注册的SOAP类
        /// </summary>
        /// <param name="marks"></param>
        /// <returns></returns>
        private static List<(Type autoSoap, TvAutoSoapVersion version, string url)> GetAutoSoapClasses(string[] marks)
        {
            var result = new List<(Type, TvAutoSoapVersion, string)>();

            foreach (var loadedType in TvAssembly.GetLoadedAssemblies())
            {
                var autoSoapAttr = loadedType.GetCustomAttribute<TvAutoSoapAttribute>();
                var serviceContractAttr = loadedType.GetCustomAttribute<ServiceContractAttribute>();
                if (!loadedType.IsClass || loadedType.IsAbstract || autoSoapAttr == null || serviceContractAttr == null)
                {
                    continue;
                }

                if (autoSoapAttr._marks.Length > 0 && !marks.Any(x => autoSoapAttr._marks.Contains(x)))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(autoSoapAttr._url))
                {
                    continue;
                }

                var allInterfaces = loadedType.GetInterfaces();
                var baseInterfaces = allInterfaces.AsParallel().SelectMany(x => x.GetInterfaces());
                var interfaces = allInterfaces.Except(baseInterfaces).ToList();
                if (interfaces.Count > 0)
                {
                    continue;
                }

                result.Add((loadedType, autoSoapAttr._version, autoSoapAttr._url));
            }

            return result;
        }
    }
}