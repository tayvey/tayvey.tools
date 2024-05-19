# Tayvey.Tools

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

## 数据转换

### IPEndPoint

```c#
using Tayvey.Tools;

// IP端点
var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);

// 转 IP地址 + 端口号
(string? ipAddress, ushort? port) = ipEndPoint.TryParseToTuple();
```

## 加密

### MD5 32位

```c#
// 明文
var str = "tayvey.tools";

// 加密. 密文是否大写:false
str = str.MD5Encryption(false);
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

TvAutoDI.Init();
var iTest = TvAutoDI.Get<ITest>();
iTest?.Print();
```
