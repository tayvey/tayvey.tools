using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// SWAGGER扩展
    /// </summary>
    public static class TvSwagger
    {
        /// <summary>
        /// 配置
        /// </summary>
        private static TvSwaggerConfig? _config;

        /// <summary>
        /// 添加SWAGGER服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTvSwagger(this IServiceCollection services, TvSwaggerConfig config)
        {
            _config = config;

            services.AddSwaggerGen(opt =>
            {
                // Swagger文档基础信息
                opt.SwaggerDoc(config.Name, new OpenApiInfo
                {
                    Version = config.Version,
                    Title = config.Title
                });
                opt.OrderActionsBy(o => o.RelativePath);

                // 添加XML文件
                foreach (var assemblyName in config.AssemblyNames)
                {
                    // 拼接路径并检查文件是否存在
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
                    if (!File.Exists(xmlPath))
                    {
                        continue;
                    }

                    opt.IncludeXmlComments(xmlPath, true);
                }

                // 配置自定义请求头
                foreach (var header in config.Headers)
                {
                    opt.AddSecurityDefinition(header.Key, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = header.Desc,
                        Name = header.Key,
                        Type = SecuritySchemeType.ApiKey
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = header.Key
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                }
            });
        }

        /// <summary>
        /// 使用SWAGGER
        /// </summary>
        /// <param name="app"></param>
        public static void UseTvSwagger(this IApplicationBuilder app)
        {
            if (_config == null)
            {
                throw new Exception($"SWAGGER初始化异常. 配置异常.");
            }

            // 使用Swagger
            app.UseSwagger();

            // 使用SwaggerUI
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint($"/swagger/{_config.Name}/swagger.json", _config.Name);
                opt.RoutePrefix = "swagger";
            });
        }
    }
}