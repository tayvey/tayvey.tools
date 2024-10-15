using Tayvey.Tools.Extensions;
using Tayvey.Tools.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllers();

// 配置服务
builder.Services.AddTvConfig(builder.Configuration);
// SOAP服务
builder.Services.AddTvSoap();
// MONGODB服务
var tvMongoConfigs = builder.Configuration.GetSection("mongodb").Get<TvMongoConnConfig[]>() ?? [];
builder.Services.AddTvMongo(tvMongoConfigs);
// REDIS服务
var tvRedisConfigs = builder.Configuration.GetSection("redis").Get<TvRedisConnConfig[]>() ?? [];
builder.Services.AddTvRedis(tvRedisConfigs);
// SWAGGER服务
var tvSwaggerConfig = builder.Configuration.GetSection("swagger").Get<TvSwaggerConfig>() ?? new();
builder.Services.AddTvSwagger(tvSwaggerConfig);
// 自动依赖注入服务
builder.Services.AddTvAutoDI();
#endregion

var app = builder.Build();

#region App
// 全局异常
app.UseTvGlobalEx();
// 自动SOAP注册
app.UseTvSoap();
// SWAGGER注册
app.UseTvSwagger();
// 自动中间件注册
app.UseTvAutoMw();

app.MapControllers();
app.Run(); 
#endregion