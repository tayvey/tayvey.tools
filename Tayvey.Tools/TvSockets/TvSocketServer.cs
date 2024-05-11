using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tayvey.Tools.TvSockets.Models;

namespace Tayvey.Tools.TvSockets
{
    /// <summary>
    /// TvSocket服务端
    /// </summary>
    public class TvSocketServer
    {
        /// <summary>
        /// Socket连接
        /// </summary>
        private Socket Socket { get; }

        /// <summary>
        /// 客户端连接池
        /// </summary>
        private ConcurrentDictionary<string, TvSocketClient> Clients { get; }

        /// <summary>
        /// 连接协议
        /// </summary>
        public ProtocolType ProtoType { get; }

        /// <summary>
        /// 服务端IP地址
        /// </summary>
        public string? ServerIpAddress { get; private set; }

        /// <summary>
        /// 服务端端口号
        /// </summary>
        public ushort? ServerPort { get; private set; }

        /// <summary>
        /// 客户端连接池最大连接数量
        /// </summary>
        public uint MaxConnections { get; set; } = 1024;

        /// <summary>
        /// 发送缓存区大小
        /// </summary>
        public int? SendBufferSize
        {
            get => GetBufferSize(true);
            set => SetBufferSize(true, value);
        }

        /// <summary>
        /// 接收缓存区大小
        /// </summary>
        public int? ReceiveBufferSize
        {
            get => GetBufferSize(false);
            set => SetBufferSize(false, value);
        }

        /// <summary>
        /// 自定义日志记录
        /// </summary>
        public Func<TvSocketLog, Task>? CustomLogging { get; set; }

        /// <summary>
        /// 释放连接回调
        /// </summary>
        public Func<TvSocketServer, Task>? DisposeCallBack { get; set; }

        /// <summary>
        /// 接收数据回调
        /// </summary>
        public Func<TvSocketReceive, Task<string>>? ReceiveCallBack { get; set; }

        /// <summary>
        /// 服务端初始化构造
        /// </summary>
        /// <param name="protoType">连接协议</param>
        public TvSocketServer(ProtocolType protoType)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protoType);
            Clients = new ConcurrentDictionary<string, TvSocketClient>();
            ProtoType = protoType;
        }

        /// <summary>
        /// 获取发送/接收缓存区
        /// </summary>
        /// <param name="isSend">是否获取发送缓存区, 反之获取接收缓存区</param>
        /// <returns></returns>
        private int? GetBufferSize(bool isSend)
        {
            try
            {
                return isSend ? Socket.SendBufferSize : Socket.ReceiveBufferSize;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置发送/接收缓存区
        /// </summary>
        /// <param name="isSend">是否设置发送缓存区, 反之设置接收缓存区</param>
        /// <param name="value">设置值</param>
        private void SetBufferSize(bool isSend, int? value)
        {
            try
            {
                if (value == null || value <= 0)
                {
                    return;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    value /= 2;
                }

                if (isSend)
                {
                    Socket.SendBufferSize = value!.Value;
                }
                else
                {
                    Socket.ReceiveBufferSize = value!.Value;
                }
            }
            catch { }
        }

        /// <summary>
        /// 启动服务端
        /// </summary>
        /// <param name="serverIpAddress">服务端IP地址</param>
        /// <param name="serverPort">服务端端口号</param>
        /// <returns></returns>
        public Task<bool> StartAsync(string serverIpAddress, ushort serverPort) => Task.Run(async () =>
        {
            ServerIpAddress = serverIpAddress;
            ServerPort = serverPort;

            try
            {
                Socket.Bind(new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort));
                Socket.Listen((int)MaxConnections);

                Socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

                await LoggingAsync("服务端启动成功");

                return true;
            }
            catch (Exception e)
            {
                await DisposeAsync(false);

                await LoggingAsync("服务端启动异常", e: e);

                return false;
            }
        });

        /// <summary>
        /// 接受连接回调
        /// </summary>
        /// <param name="ar">异步参数</param>
        private void AcceptCallBack(IAsyncResult ar) => Task.Run(async () =>
        {
            try
            {
                await AddToClientsAsync(Socket.EndAccept(ar));

                Socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            catch
            {
                return;
            }
        }).Wait();

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="clientIpAddress">客户端IP地址</param>
        /// <param name="clientPort">客户端端口号</param>
        /// <param name="e">异常</param>
        /// <returns></returns>
        private Task LoggingAsync(
            string text,
            string? clientIpAddress = null,
            ushort? clientPort = null,
            Exception? e = null
        ) => Task.Run(async () =>
        {
            try
            {
                if (CustomLogging != null)
                {
                    await CustomLogging(new TvSocketLog(text, this, clientIpAddress, clientPort, e));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"日志处理异常 [{e.Message}][{e.StackTrace}]");
            }
        });

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="isCallBack">是否触发释放连接回调</param>
        /// <returns></returns>
        private Task DisposeAsync(bool isCallBack) => Task.Run(async () =>
        {
            try
            {
                var tasks = Clients.AsParallel().Select(async client =>
                {
                    await client.Value.DisposeAsync(false);
                    _ = Clients.TryRemove(client.Key, out _);
                });
                await Task.WhenAll(tasks);

                if (Socket.Connected)
                {
                    Socket.Disconnect(false);
                }

                Socket.Close();

                Socket.Dispose();

                if (isCallBack && DisposeCallBack != null)
                {
                    await DisposeCallBack(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"释放连接异常 [{e.Message}][{e.StackTrace}]");
            }
        });

        /// <summary>
        /// 添加客户端到连接池中
        /// </summary>
        /// <param name="socket">客户端</param>
        /// <returns></returns>
        private async Task AddToClientsAsync(Socket socket)
        {
            var client = new TvSocketClient(socket, Socket.LocalEndPoint)
            {
                CustomLogging = ClientLoggingCallBackAsync,
                DisposeCallBack = ClientDisposeCallBackAsync,
                ReceiveCallBack = ReceiveCallBack
            };

            if (string.IsNullOrWhiteSpace(client.ClientIpAddress))
            {
                return;
            }

            if (Clients.Count >= MaxConnections)
            {
                await client.DisposeAsync(false);
                await LoggingAsync("连接池已达上限. 已主动断开新加入的客户端", client.ClientIpAddress, client.ClientPort);
                return;
            }

            if (!Clients.TryAdd($"{client.ClientIpAddress}:{client.ClientPort}", client))
            {
                await client.DisposeAsync(false);
                await LoggingAsync("未能成功加入到连接池中. 已主动断开与客户端的连接", client.ClientIpAddress, client.ClientPort);
                return;
            }

            await LoggingAsync($"与客户端成功建立连接, 连接池空间({Clients.Count}/{MaxConnections})", client.ClientIpAddress, client.ClientPort);
            await client.BeginReceiveAsync();
        }

        /// <summary>
        /// 客户端日志记录回调
        /// </summary>
        /// <param name="log">TvSocket客户端日志对象</param>
        /// <returns></returns>
        private Task ClientLoggingCallBackAsync(TvSocketLog log) => Task.Run(async () =>
        {
            try
            {
                if (CustomLogging != null)
                {
                    await CustomLogging(log);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"日志处理异常 [{e.Message}][{e.StackTrace}]");
            }
        });

        /// <summary>
        /// 客户端释放连接回调
        /// </summary>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        private Task ClientDisposeCallBackAsync(TvSocketClient client) => Task.Run(() =>
        {
            _ = Clients.TryRemove($"{client.ClientIpAddress}:{client.ClientPort}", out _);
        });
    }
}
