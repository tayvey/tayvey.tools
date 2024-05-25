#if NETSTANDARD2_1
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Tayvey.Tools.TvAutoDIs;
using Tayvey.Tools.TvConfigs;
#endif

namespace Tayvey.Tools.TvSwaggers
{
    /// <summary>
    /// TvSwagger服务
    /// </summary>
    public static class TvSwagger
    {
        /// <summary>
        /// 添加TvSwagger服务
        /// </summary>
        /// <param name="service"></param>
        public static void AddTvSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(opt =>
            {
                // Swagger文档基础信息
                opt.SwaggerDoc(TvConfig.Options.TvSwagger.Name, new OpenApiInfo
                {
                    Version = TvConfig.Options.TvSwagger.Version,
                    Title = TvConfig.Options.TvSwagger.Name
                });
                opt.OrderActionsBy(o => o.RelativePath);

                // 获取程序集列表
                var assemblyPq = TvAutoDI.GetAssemblies();

                // 添加XML文件
                foreach (var assembly in assemblyPq)
                {
                    // 拼接路径并检查文件是否存在
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                    if (!File.Exists(xmlPath))
                    {
                        continue;
                    }

                    opt.IncludeXmlComments(xmlPath, true);
                }

                // 配置自定义请求头
                foreach (var header in TvConfig.Options.TvSwagger.Headers)
                {
                    opt.AddSecurityDefinition(header.Key, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = header.Desc,
                        Name = header.Key,
                        Type = SecuritySchemeType.ApiKey
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
                    }});
                }

                // 枚举注释
                opt.SchemaFilter<EnumSchemaFilter>();
            });
        }

        /// <summary>
        /// 使用TvSwagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseTvSwagger(this IApplicationBuilder app)
        {
            // 使用Swagger
            app.UseSwagger();

            // 名称
            var name = TvConfig.Options.TvSwagger.Name;

            // 使用SwaggerUI
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint($"/swagger/{name}/swagger.json", name);
                opt.RoutePrefix = "swagger";
            });
        }
    }
}
