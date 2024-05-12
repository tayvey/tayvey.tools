# Tayvey.Tools

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
using Tayvey.Tools.TvTasks;
using Tayvey.Tools.TvTasks.Models;

// 列表
var range = Enumerable.Range(0, 1000);

// 异步遍历
await range.TvForEachAsync(new TvForEachOptions<int>
{
    TryAction = item => { }, // 处理
    CatchAction = (item, ex) => { }, // 异常处理
    MaxConcurrency = 20, // 最大异步数量
    IsGroup = false // 是否分组遍历
});
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
