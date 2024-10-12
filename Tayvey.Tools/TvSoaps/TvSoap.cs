//#if NETSTANDARD2_1
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using SoapCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using Tayvey.Tools.Service;
//using Tayvey.Tools.TvConfigs;
//#endif

//using Tayvey.Tools.Service;

//namespace Tayvey.Tools.TvSoaps
//{
//    /// <summary>
//    /// TvSoap
//    /// </summary>
//    public static class TvSoap
//    {
//        /// <summary>
//        /// 注册SOAP的类
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private static readonly Lazy<List<Type>> Classes = new(LzClasses);
//#else
//        private static readonly Lazy<List<Type>> Classes = new Lazy<List<Type>>(LzClasses);
//#endif

//        /// <summary>
//        /// 懒加载注册SOAP的类
//        /// </summary>
//        /// <returns></returns>
//        private static List<Type> LzClasses()
//        {
//            return TvRelyAssembly.LzRelyAssemblyTypes.Value.AsParallel()
//                .Where(i => i.IsClass && !i.IsAbstract && i.GetCustomAttribute<ServiceContractAttribute>() != null)
//                .ToList();
//        }

//        /// <summary>
//        /// 添加TvSoap
//        /// </summary>
//        /// <param name="service"></param>
//        /// <returns></returns>
//        public static IServiceCollection AddTvSoap(this IServiceCollection service)
//        {
//            // SOAP服务
//            service.AddSoapCore();

//            return service;
//        }

//        /// <summary>
//        /// 使用TvSoap
//        /// </summary>
//        /// <param name="app"></param>
//        /// <returns></returns>
//        public static IApplicationBuilder UseTvSoap(this IApplicationBuilder app)
//        {
//            // 遍历使用SOAP
//            foreach (var item in Classes.Value)
//            {
//                // 获取配置
//                var config = TvConfig.Options.TvSoap.FirstOrDefault(i => i.ClassName == item.Name);
//                if (config == null)
//                {
//                    continue;
//                }

//                // 使用SOAP
//                app.UseSoapEndpoint(item, $"/soap/{item.Name}.asmx", new SoapEncoderOptions
//                {
//                    MessageVersion = config.Version switch
//                    {
//                        12 => MessageVersion.Soap12WSAddressingAugust2004,
//                        _ => MessageVersion.Soap11WSAddressingAugust2004
//                    }
//                }, caseInsensitivePath: true);
//            }

//            return app;
//        }
//    }
//}