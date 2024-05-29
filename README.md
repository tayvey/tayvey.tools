# Tayvey.Tools

## TvTools

### 服务&管道

```c#
using Tayvey.Tools;

var builder = WebApplication.CreateBuilder(args);

// service
var service = builder.Services;
service.AddControllers();
service.AddTvTools(builder.Configuration);

// app
var app = builder.Build();
app.UseTvTools();

app.MapControllers();
app.Run();
```

## Api返回

WebApi返回

```c#
using Tayvey.Tools.TvApiResults.Models;

[HttpGet]
public IActionResult Get()
{
    // 返回成功 - 返回消息, 返回数据
    return TvApiResult.Ok("成功", new { });

    // 返回失败
    return TvApiResult.Fail("失败", new { });

    // 返回鉴权失败
    return TvApiResult.Unauthorized("鉴权失败", new { });

    // 返回资源不存在
    return TvApiResult.NotFound("资源不存在", new { });
    
    // 返回方法不被允许
    return TvApiResult.MethodNotAllowed("方法不被允许", new { });

    // 返回异常
    return TvApiResult.Error("异常", new { });
}
```

## 自动依赖注入

### 初始化

服务接口

```C#
using Tayvey.Tools.TvAutoDIs.Attrs;
using Tayvey.Tools.TvAutoDIs.Enums;

/// <summary>
/// lifeCycle: 生命周期(默认Scoped), ignoreInterface: 忽略接口(默认false)
/// </summary>
[TvAutoDI(lifeCycle: TvAutoDILifeCycle.Scoped, ignoreInterface: false)]
public class Test : ITest
{
    void ITest.Print()
    {
        Console.WriteLine("Test");
    }
}

public interface ITest
{
    void Print();
}
```

WebApi初始化

```C#
using Tayvey.Tools.TvAutoDIs;

var builder = WebApplication.CreateBuilder(args);

var service = builder.Services;
service.AddTvAutoDI();
```

自定义初始化

```C#
using Tayvey.Tools.TvAutoDIs;

var iTest = TvAutoDI.Get<ITest>();
iTest?.Print();
```

## 配置

### 初始化

WebApi初始化

```C#
using Tayvey.Tools.TvConfigs;

var builder = WebApplication.CreateBuilder(args);

// 初始化
TvConfig.InitConfiguration(builder.Configuration);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
```

自定义初始化

```c#
using Tayvey.Tools.TvConfigs;

// 配置文件
var file1 = Path.Combine(AppContext.BaseDirectory, "config.json");
var file2 = Path.Combine(AppContext.BaseDirectory, "config.test.json");

// 初始化
TvConfig.InitConfiguration(file1, file2);
```

### 读取配置

配置示例

```json
{
  "Custom": {
    "Str": "str",
    "List": ["str"]
  }
}
```

读取配置

```C#
using Tayvey.Tools.TvConfigs;

// 自定义配置
TvConfig.Get<string>("Custom:Str");
TvConfig.Get<List<string>>("Custom:List");
```

## Excel

### 读取Excel

```c#
using Tayvey.Tools.TvExcels;

// excel文件
var path = Path.Combine(@"C:\Users\administrator\Desktop\test.xlsx");
using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

// 读取
var cells = await TvExcel.ReadAsync(fs);

// 筛选工作表 (1开始)
var cellsPq = cells.AsParallel().Where(i => i.Worksheet == 1);

// 筛选行 (1开始)
cellsPq = cellsPq.Where(i => i.Row == 4);

// 筛选列 (1开始)
var cell = cellsPq.FirstOrDefault(i => i.Col == 1);

// 读取值
var value = cell?.Value;
```

## 中间件

### 全局异常中间件

```c#
using Tayvey.Tools.TvMiddlewares.Middlewares;

public class CustomGlobalEx : TvMiddlewareGlobalEx
{
    public CustomGlobalEx(RequestDelegate next) : base(next)
    {
        base.Process = Process;
    }

    private static void Process(Exception e)
    {
        // 自定义记录异常
    }
}
```

### HTTP状态码处理中间件

```c#
using Tayvey.Tools.TvMiddlewares.Middlewares;

public class CustomHttpStatusCode : TvMiddlewareHttpStatusCode
{
    public CustomHttpStatusCode(RequestDelegate next) : base(next)
    {
        base.Process = Process;
    }

    private static void Process(HttpContext context)
    {
        // 自定义处理
    }
}
```

### 自动注册中间件

#### 配置

```json
"TvConfig": {
  "TvMiddleware": [
    "Test", // 中间件类名, 按顺序注册
    "Test1"
  ]
}
```

#### 自定义中间件

```C#
using Tayvey.Tools.TvMiddlewares.Middlewares;

// 必须继承 TvMiddlewareBase 才能被自动注册
public class CustomMiddleware(RequestDelegate next) : TvMiddlewareBase(next)
{
    public override Task InvokeAsync(HttpContext context)
    {
        // 自定义处理
    }
}
```

#### 使用自动注册

```C#
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvMiddlewares;

var builder = WebApplication.CreateBuilder(args);

// 初始化配置 (需要读取中间件配置)
TvConfig.InitConfiguration(builder.Configuration);

// service
var service = builder.Services;
service.AddControllers();

// app
var app = builder.Build();

// 自动注册中间件
app.UseTvMiddleware();

app.MapControllers();
app.Run();
```

## MongoDB

### 配置

```json
"TvConfig": {
  "TvMongo": [
    {
      "Key": "test",
      // readPreference=secondaryPreferred 读写分离
      "ConnectionStr": "mongodb://username:password@host:port/?readPreference=secondaryPreferred" 
    }
  ]
}
```

### 获取集合

#### 实体

```c#
using Tayvey.Tools.TvMongos;
using Tayvey.Tools.TvMongos.Attrs;
using Tayvey.Tools.TvMongos.Models;

[TvMongo("Test")]
internal class Test : TvMongoDataBase
{
}

/*
    获取集合
    <T>: 实体对象, 必须继承TvMongoDataBase抽象类
    dbName: 指定数据库
    collectionName: 指定集合 (可省略)
        如果传入, 则以传入的collectionName为准
        如果省略, 则以实体特性TvMongo中指定的为准
        如果省略且未使用实体特性TvMongo指定, 则以实体名称为准
    key: 指定配置Key (可省略)
        如果传入, 则以传入的key为准
        如果省略, 则默认配置中的第一个key
 */
var collection = TvMongo.GetCollection<Test>("dbName", "collectionName", "test");

// 查询数据
var data = await collection.Find(i => i.Id == ObjectId.Empty).ToListAsync();
```

#### Bson

```C#
using Tayvey.Tools.TvMongos;

/*
    获取集合
    dbName: 指定数据库
    collectionName: 指定集合
    key: 指定连接KEY (可省略)
        如果传入, 则以传入的key为准
        如果省略, 则默认配置中的第一个key
 */
var collection = TvMongo.GetCollection("dbName", "collectionName", "test");

// 查询数据
var data = await collection.Find(i => i["_id"] == ObjectId.Empty).ToListAsync();
```

## Redis

### 配置

```json
"TvConfig": {
  "TvRedis": [
    {
      "Key": "test",
      // defaultDatabase=dbNumber 指定默认连接库编号
      "ConnectionStr": "host:port,password=password,defaultDatabase=dbNumber"
    }
  ]
}
```

### 获取连接客户端

```c#
using Tayvey.Tools.TvRedises;

/*
 获取连接客户端
 key: 指定配置Key (可省略)
    如果传入, 则以传入的key为准
    如果省略, 则默认配置中的第一个key
 */
var client = TvRedis.GetClient("test");

// 获取服务器列表
_ = client.GetServers();
```

### 获取连接库

```C#
using StackExchange.Redis;
using Tayvey.Tools.TvRedises;

/*
 获取连接客户端
 key: 指定配置Key (可省略)
    如果传入, 则以传入的key为准
    如果省略, 则默认配置中的第一个key
 db: 指定连接库编号 (可省略)
    如果传入, 则以传入的db为准 (负数等同于省略)
    如果省略, 并且在连接字符串中指定了defaultDatabase=dbNumber, 则以dbNumber为准
    如果省略, 但未指定defaultDatabase=dbNumber, 则以0为准
 */
var db = TvRedis.GetDatabase("test", -1);

// 获取string数据
_ = await db.StringGetAsync("test", CommandFlags.PreferReplica);
```

## SOAP

### 配置

```json
"TvConfig": {
  "TvSoap": [
    {
      "ClassName": "SoapTest", // 类名
      "Version": 12 // 11 | 12, 非11和12将默认11
    }
  ]
}
```

### SOAP

```C#
using System.ServiceModel;

// 访问路径: /soap/[className].asmx
[ServiceContract]
public class SoapTest
{
    [OperationContract]
    public string Get()
    {
        return "";
    }
}
```

### 服务&管道

```C#
using Tayvey.Tools.TvSoaps;

var builder = WebApplication.CreateBuilder(args);

// 初始化配置 (需要读取SOAP配置)
TvConfig.InitConfiguration(builder.Configuration);

// service
var service = builder.Services;
service.AddControllers();

// SOAP服务
service.AddTvSoap();

// app
var app = builder.Build();

// SOAP管道
app.UseTvSoap();

app.MapControllers();
app.Run();
```

## Socekt 服务端/客户端

### 启动&停止服务端

```c#
using Tayvey.Tools.TvSockets;

// 创建服务端对象, TCP协议
var server = new TvSocketServer(ProtocolType.Tcp)
{
    MaxConnections = 1024, // 客户端最大连接数量
    SendBufferSize = 1024, // 发送缓存区大小 (字节)
    ReceiveBufferSize = 1024, // 接受缓存区大小 (字节)
    CustomLogging = log => // 自定义日常处理 (异步)
    {
        return Task.CompletedTask;
    },
    DisposeCallBack = server => // 服务端停止&释放回调函数
    {
        return Task.CompletedTask;
    },
    ReceiveCallBack = receive => // 接收客户端发来的数据回调函数, 返回粘包数据字符串
    {
        return Task.FromResult("");
    }
};

// 启动服务端 绑定IP地址&端口号
if (await server.StartAsync("127.0.0.1", 6666))
{
    // 启动成功
}
else
{
    // 启动失败|异常
}

// 停止服务端
await server.StopAsync();
```

### 启动&停止客户端

```c#
using Tayvey.Tools.TvSockets;

// 创建客户端对象, TCP协议
var client = new TvSocketClient(ProtocolType.Tcp)
{
    SendBufferSize = 1024, // 发送缓存区大小 (字节)
    ReceiveBufferSize = 1024, // 接受缓存区大小 (字节)
    CustomLogging = log => // 自定义日常处理 (异步)
    {
        return Task.CompletedTask;
    },
    DisposeCallBack = server => // 客户端停止&释放回调函数
    {
        return Task.CompletedTask;
    },
    ReceiveCallBack = receive => // 接收服务端发来的数据回调函数, 返回粘包数据字符串
    {
        return Task.FromResult("");
    }
};

// 启动客户端 连接到IP地址&端口号
if (await client.StartAsync("127.0.0.1", 6666))
{
    // 启动成功 开始接收数据
    await client.BeginReceiveAsync();
}
else
{
    // 启动失败|异常
}

// 停止客户端
await client.StopAsync();
```

## Swagger

### Swagger配置

```json
"TvConfig": {
  "TvSwagger": {
    "Name": "测试Swagger", // 默认swagger
    "Version": "测试 - 1.0", // 默认1.0
    "Headers": [ // 默认[]
      {
        "Key": "Tv-Test1", // 请求头KEY
        "Desc": "..." // 请求头说明
      },
      {
        "Key": "Tv-Test2",
        "Desc": "..."
      }
    ]
  }
}
```

### 应用Swagger

```C#
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvSwaggers;

var builder = WebApplication.CreateBuilder(args);

// 初始化Tv配置 (读取swagger配置需要使用)
TvConfig.InitConfiguration(builder.Configuration);

// service
var service = builder.Services;
service.AddControllers();

// TvSwagger
service.AddTvSwagger();

// app
var app = builder.Build();

// TvSwagger
app.UseTvSwagger();

app.MapControllers();
app.Run();
```

## 定时任务

### 自动注册定时任务

#### 配置

```json
"TvConfig": {
  "TvTimedTask": [
    {
      "ClassName": "Test", // 定时任务类名
      "Cron": "0/10 * * * * ? " // cton表达式
    }
  ]
}
```

#### 任务

```c#
using Quartz;

public class Test : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // 执行任务
    }
}
```

#### WebApi项目自动注册

```c#
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvTimedTasks;

var builder = WebApplication.CreateBuilder(args);

// 初始化配置 (需要读取定时任务配置)
TvConfig.InitConfiguration(builder.Configuration);

// service
var service = builder.Services;
service.AddControllers();

// 自动注册定时任务
service.AddTvTimedTask();

// app
var app = builder.Build();

app.MapControllers();
app.Run();
```

#### CMD项目自动注册

```c#
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvTimedTasks;

// 配置文件
var file1 = Path.Combine(AppContext.BaseDirectory, "config.json");

// 初始化配置 (需要读取定时任务配置)
TvConfig.InitConfiguration(file1);

// 初始化并启动定时任务
TvTimedTask.Init();
```

## 数据转换

### IPEndPoint

#### 转IP地址 + 端口号元组

```c#
using Tayvey.Tools;

// IP端点
var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);

// 转换
(string? ipAddress, ushort? port) = ipEndPoint.TvtpToTuple(); // ("127.0.0.1", 6666)
```

### string

#### 转日期时间

```c#
using Tayvey.Tools;

// 日期时间字符串
var dateTime = "2024-05-19 19:08:02";
var dateTime1 = "20240519190802";

// 转换
DateTime? dt = dateTime.TvtpToDateTime(); // 2024-05-19 19:08:02
DateTime? dt1 = dateTime1.TvtpToDateTime("yyyyMMddHHmmss"); // 20240519190802
```

#### 转int32

```c#
using Tayvey.Tools;

// int32字符串
var int32 = "12345";

// 转换
int? dt = int32.TvtpToInt32(); // 12345
```

#### 转枚举项

```c#
using Tayvey.Tools;

/// <summary>
/// 枚举对象
/// </summary>
public enum Test
{
    None = 0
}

// 枚举项字符串
var enumStr = "None";

// 转换
Test? none = enumStr.TvtpToEnum<Test>(); // None
```

#### 转float

```c#
using Tayvey.Tools;

// float字符串
var floatStr = "0.1";

// 转换
float? f = floatStr.TvtpToFloat(); // 0.1
```

#### 转decimal

```c#
using Tayvey.Tools;

// decimal字符串
var decimalStr = "0.1";

// 转换
decimal? d = decimalStr.TvtpToDecimal(); // 0.1
```

#### 转TimeSpan

```c#
using Tayvey.Tools;

// TimeSpan字符串
var timespanStr = "00:00:00";
var timespanStr1 = "000000";

// 转换
TimeSpan? t = timespanStr.TvtpToTimeSpan(); // 00:00:00
TimeSpan? t1 = timespanStr.TvtpToTimeSpan("HHmmss"); // 000000
```

### DateTime

#### 转日期时间字符串

```c#
using Tayvey.Tools;

// 日期时间
var now = DateTime.Now;

// 转换
string? dt = now.TvtpToString(); // 2024-05-19 19:08:02
string? dt1 = now.TvtpToString("yyyyMMddHHmmss"); // 20240519190802
```

### decimal

#### 转decimal字符串

```c#
using Tayvey.Tools;

// decimal
var d = 0.100000m;

// 转换
string? dStr = d.TvtpToString(); // 0.1
string? dStr1 = d.TvtpToString(""); // 0.100000
```

### Enum

#### 转枚举项索引字符串

```c#
using Tayvey.Tools;

/// <summary>
/// 枚举对象
/// </summary>
public enum Test
{
    None = 0
}

// 枚举项
var item = Test.None;

// 转换
string? itemStr = item.TvtpToString(); // "0"
```

## 加密

### MD5 32位

```c#
using Tayvey.Tools;

// 明文
var str = "tayvey.tools";

// 加密. 密文是否大写:false
str = str.MD5Encryption(false);
```

## 模型校验

### 控制器

```c#
using Microsoft.AspNetCore.Mvc;
using Tayvey.Tools.TvApiResults.Models;

[Route("api/[Controller]/[Action]")]
[ApiController] // 需要使用这个特性
public class TestController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(TestReq req)
    {
        return TvApiResult.Ok("成功", req);
    }
}
```

### 添加模型校验

```c#
using Tayvey.Tools;

var builder = WebApplication.CreateBuilder(args);

// service
var service = builder.Services;
service.AddControllers();

// 模型校验
service.AddTvModelState();

// app
var app = builder.Build();

app.MapControllers();
app.Run();
```

## 异步

### 异步遍历

```C#
using Tayvey.Tools;

// 列表
var range = Enumerable.Range(0, 1000);

// 异步遍历 
await range.TvForEachAsync(item =>
{
    try
    {
        // 处理逻辑
    }
    catch (Exception)
    {
        // 异常处理逻辑
    }
}, 20, false); // 最大异步数量(默认null), 是否分组遍历(默认false)
```
