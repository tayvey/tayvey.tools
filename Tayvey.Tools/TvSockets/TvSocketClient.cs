using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tayvey.Tools.TvSockets.Enums;
using Tayvey.Tools.TvSockets.Models;

namespace Tayvey.Tools.TvSockets
{
    /// <summary>
    /// TvSocket客户端
    /// </summary>
    public class TvSocketClient
    {
        /// <summary>
        /// Socket连接
        /// </summary>
        private Socket Socket { get; }

        /// <summary>
        /// 粘包字符串
        /// </summary>
        internal string DipBag { get; private set; } = "";

        /// <summary>
        /// 接收数据字节数组
        /// </summary>
        internal byte[] ReceiveBuffer { get; private set; }

        /// <summary>
        /// Socket身份
        /// </summary>
        internal TvSocketRole Role { get; }

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
        /// 客户端IP地址
        /// </summary>
        public string? ClientIpAddress { get; private set; }

        /// <summary>
        /// 客户端端口号
        /// </summary>
        public ushort? ClientPort { get; private set; }

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
        public Func<TvSocketClient, Task>? DisposeCallBack { get; set; }

        /// <summary>
        /// 接收数据回调
        /// </summary>
        public Func<TvSocketReceive, Task<string>>? ReceiveCallBack { get; set; }

        /// <summary>
        /// 客户端初始化构造
        /// </summary>
        /// <param name="protoType">连接协议</param>
        public TvSocketClient(ProtocolType protoType)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protoType);
            ReceiveBuffer = new byte[Socket.ReceiveBufferSize];
            Role = TvSocketRole.Client;
            ProtoType = protoType;
        }

        /// <summary>
        /// 服务端的子客户端初始化构造
        /// </summary>
        /// <param name="socket">socket服务端的子客户端</param>
        /// <param name="endPoint">服务端的端点</param>
        internal TvSocketClient(Socket socket, EndPoint? endPoint)
        {
            Socket = socket;

            try
            {
                ReceiveBuffer = new byte[socket.ReceiveBufferSize];
                Role = TvSocketRole.ServerChildClient;
                ProtoType = socket.ProtocolType;

                (ServerIpAddress, ServerPort) = ((IPEndPoint?)endPoint).TryParseToTuple();
                (ClientIpAddress, ClientPort) = ((IPEndPoint?)socket.RemoteEndPoint).TryParseToTuple();
            }
            catch (Exception e)
            {
                ReceiveBuffer = new byte[4];

                DisposeAsync(false).Wait();
                LoggingAsync("初始化客户端异常", e).Wait();
            }
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
        /// 释放连接
        /// </summary>
        /// <param name="isCallBack">是否触发释放连接回调</param>
        /// <returns></returns>
        internal Task DisposeAsync(bool isCallBack) => Task.Run(async () =>
        {
            try
            {
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
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"释放连接异常 [{e.Message}][{e.StackTrace}]");
            }
        });

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="e">异常信息</param>
        /// <returns></returns>
        public Task LoggingAsync(string text, Exception? e = null) => Task.Run(async () =>
        {
            try
            {
                if (CustomLogging != null)
                {
                    await CustomLogging(new TvSocketLog(text, this, e));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"日志处理异常 [{e.Message}][{e.StackTrace}]");
            }
        });

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="serverIpAddress">服务端IP地址</param>
        /// <param name="serverPort">服务端端口号</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string serverIpAddress, ushort serverPort)
        {
            ServerIpAddress = serverIpAddress;
            ServerPort = serverPort;

            try
            {
                Socket.Connect(new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort));

                (ClientIpAddress, ClientPort) = ((IPEndPoint?)Socket.LocalEndPoint).TryParseToTuple();

                await LoggingAsync("与服务端成功建立连接");

                return true;
            }
            catch (Exception e)
            {
                await DisposeAsync(false);

                await LoggingAsync("与服务端建立连接异常", e);

                return false;
            }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <returns></returns>
        public async Task BeginReceiveAsync()
        {
            if (!await HeartbeatDetectionAsync())
            {
                return;
            }

            ReceiveBuffer = new byte[Socket.ReceiveBufferSize];
            Socket.BeginReceive(ReceiveBuffer, 0, Socket.ReceiveBufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallBackAsync), null);
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        private Task<bool> HeartbeatDetectionAsync() => Task.Run(async () =>
        {
            try
            {
                var isSuc = !(Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0);
                if (isSuc)
                {
                    return true;
                }

                await DisposeAsync(true);

                await LoggingAsync("心跳检测失败, 已断开与服务端的连接");

                return false;
            }
            catch
            {
                return false;
            }
        });

        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="ar">异步参数</param>
        private void ReceiveCallBackAsync(IAsyncResult ar) => Task.Run(async () =>
        {
            try
            {
                if (!await HeartbeatDetectionAsync())
                {
                    return;
                }

                var receiveLength = Socket.EndReceive(ar);
                if (receiveLength > 0 && ReceiveCallBack != null)
                {
                    DipBag = await ReceiveCallBack(new TvSocketReceive(this, (uint)receiveLength));
                }

                await BeginReceiveAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"接收数据异常 [{e.Message}][{e.StackTrace}]");
            }
        }).Wait();

        /// <summary>
        /// 主动释放连接
        /// </summary>
        /// <returns></returns>
        public Task DisposeAsync() => Task.Run(async () =>
        {
            await DisposeAsync(true);
            await LoggingAsync("客户端已主动断开连接");
        });
    }
}
