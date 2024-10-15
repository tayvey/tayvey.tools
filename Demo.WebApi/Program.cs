using Tayvey.Tools.Extensions;
using Tayvey.Tools.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllers();

// ���÷���
builder.Services.AddTvConfig(builder.Configuration);
// SOAP����
builder.Services.AddTvSoap();
// MONGODB����
var tvMongoConfigs = builder.Configuration.GetSection("mongodb").Get<TvMongoConnConfig[]>() ?? [];
builder.Services.AddTvMongo(tvMongoConfigs);
// REDIS����
var tvRedisConfigs = builder.Configuration.GetSection("redis").Get<TvRedisConnConfig[]>() ?? [];
builder.Services.AddTvRedis(tvRedisConfigs);
// SWAGGER����
var tvSwaggerConfig = builder.Configuration.GetSection("swagger").Get<TvSwaggerConfig>() ?? new();
builder.Services.AddTvSwagger(tvSwaggerConfig);
// �Զ�����ע�����
builder.Services.AddTvAutoDI();
#endregion

var app = builder.Build();

#region App
// ȫ���쳣
app.UseTvGlobalEx();
// �Զ�SOAPע��
app.UseTvSoap();
// SWAGGERע��
app.UseTvSwagger();
// �Զ��м��ע��
app.UseTvAutoMw();

app.MapControllers();
app.Run(); 
#endregion