//#if NETSTANDARD2_1
//using System;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//#endif

//namespace Tayvey.Tools.TvSockets
//{
//    /// <summary>
//    /// TvTcp客户端
//    /// </summary>
//    public class TvTcpClient : IDisposable
//    {
//        /// <summary>
//        /// Socket
//        /// </summary>
//        private Socket _socket;

//        /// <summary>
//        /// 保持连接
//        /// </summary>
//        private bool _keepAlive = false;

//        /// <summary>
//        /// 服务端地址
//        /// </summary>
//        private string? _serverHost;

//        /// <summary>
//        /// 服务端端口
//        /// </summary>
//        private int? _serverPort;

//        /// <summary>
//        /// 重连锁
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private readonly object _reconnectLock = new();
//#else
//        private readonly object _reconnectLock = new object();
//#endif

//        /// <summary>
//        /// 重连中
//        /// </summary>
//        private bool _reconnecting = false;

//        /// <summary>
//        /// 释放锁
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private readonly object _disposeLock = new();
//#else
//        private readonly object _disposeLock = new object();
//#endif

//        /// <summary>
//        /// 已释放
//        /// </summary>
//        private bool _disposed = false;

//        /// <summary>
//        /// 是否连接中
//        /// </summary>
//        public bool Connected
//        {
//            get
//            {
//                if (_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0)
//                {
//                    return false;
//                }

//                return _socket.Connected;
//            }
//        }

//        /// <summary>
//        /// 可读数据量
//        /// </summary>
//        public int Available => _socket.Available;

//        /// <summary>
//        /// 保持连接
//        /// </summary>
//        public bool KeepAlive
//        {
//            get
//            {
//                return _keepAlive;
//            }
//            set
//            {
//                _keepAlive = value;
//                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _keepAlive);
//            }
//        }

//        /// <summary>
//        /// 发送缓存区大小
//        /// </summary>
//        public int SendBufferSize
//        {
//            get => _socket.SendBufferSize;
//            set => _socket.SendBufferSize = value;
//        }

//        /// <summary>
//        /// 接收缓存区大小
//        /// </summary>
//        public int ReceiveBufferSize
//        {
//            get => _socket.ReceiveBufferSize;
//            set => _socket.ReceiveBufferSize = value;
//        }

//        /// <summary>
//        /// 发送超时（毫秒）
//        /// </summary>
//        public int SendTimeout
//        {
//            get => _socket.SendTimeout;
//            set => _socket.SendTimeout = value;
//        }

//        /// <summary>
//        /// 接收超时（毫秒）
//        /// </summary>
//        public int ReceiveTimeout
//        {
//            get => _socket.ReceiveTimeout;
//            set => _socket.ReceiveTimeout = value;
//        }

//        /// <summary>
//        /// 初始化构造
//        /// </summary>
//        /// <param name="addressFamily"></param>
//        /// <param name="socketType"></param>
//        public TvTcpClient(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
//        {
//            _socket = new Socket(addressFamily, socketType, ProtocolType.Tcp);
//        }

//        /// <summary>
//        /// 初始化构造
//        /// </summary>
//        /// <param name="client"></param>
//        public TvTcpClient(Socket client)
//        {
//            _socket = client;
//        }

//        /// <summary>
//        /// 析构
//        /// </summary>
//        ~TvTcpClient()
//        {
//            Dispose(false);
//        }

//        /// <summary>
//        /// 连接
//        /// </summary>
//        /// <param name="host"></param>
//        /// <param name="port"></param>
//        public void Connect(string host, int port)
//        {
//            _socket.Connect(host, port);

//            _serverHost = host;
//            _serverPort = port;
//        }

//        /// <summary>
//        /// 连接
//        /// </summary>
//        /// <param name="host"></param>
//        /// <param name="port"></param>
//        /// <returns></returns>
//        public async ValueTask ConnectAsync(string host, int port)
//        {
//            await _socket.ConnectAsync(host, port);

//            _serverHost = host;
//            _serverPort = port;
//        }

//        /// <summary>
//        /// 发送数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        public int Send(byte[] buffer) => _socket.Send(buffer);

//        /// <summary>
//        /// 发送数据
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public int SendStr(string message, Encoding encoding)
//        {
//            var buffer = encoding.GetBytes(message);
//            return Send(buffer);
//        }

//        /// <summary>
//        /// 发送数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        /// <exception cref="SocketException"></exception>
//        public async ValueTask<int> SendAsync(byte[] buffer)
//        {
//            try
//            {
//                using var source = new CancellationTokenSource();
//                if (SendTimeout > 0)
//                {
//                    source.CancelAfter(TimeSpan.FromMilliseconds(SendTimeout));
//                }

//                return await _socket.SendAsync(new ReadOnlyMemory<byte>(buffer), SocketFlags.None, source.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                throw new SocketException(10060);
//            }
//        }

//        /// <summary>
//        /// 发送数据
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public ValueTask<int> SendStrAsync(string message, Encoding encoding)
//        {
//            var buffer = encoding.GetBytes(message);
//            return SendAsync(buffer);
//        }

//        /// <summary>
//        /// 接收数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        public int Receive(byte[] buffer) => _socket.Receive(buffer);

//        /// <summary>
//        /// 接收数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public string ReceiveStr(byte[] buffer, Encoding encoding)
//        {
//            var receive = Receive(buffer);
//            if (receive == 0)
//            {
//                return "";
//            }

//            return encoding.GetString(buffer);
//        }

//        /// <summary>
//        /// 接收数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        /// <exception cref="SocketException"></exception>
//        public async ValueTask<int> ReceiveAsync(byte[] buffer)
//        {
//            try
//            {
//                using var source = new CancellationTokenSource();
//                if (ReceiveTimeout > 0)
//                {
//                    source.CancelAfter(TimeSpan.FromMilliseconds(ReceiveTimeout));
//                }

//                return await _socket.ReceiveAsync(buffer, SocketFlags.None, source.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                throw new SocketException(10060);
//            }
//        }

//        /// <summary>
//        /// 接收数据
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public async ValueTask<string> ReceiveStrAsync(byte[] buffer, Encoding encoding)
//        {
//            var receive = await ReceiveAsync(buffer);
//            if (receive == 0)
//            {
//                return "";
//            }

//            return encoding.GetString(buffer);
//        }

//        /// <summary>
//        /// 重新连接
//        /// </summary>
//        public void Reconnect()
//        {
//            if (!AcquireReconnectLock())
//            {
//                return;
//            }

//            var newSocket = ReconnectGetNewSocket();

//            try
//            {
//                newSocket.Connect(_serverHost!, _serverPort!.Value);

//                _socket.Close();
//                _socket = newSocket;
//            }
//            catch
//            {
//                newSocket.Close();
//            }
//            finally
//            {
//                _reconnecting = false;
//            }
//        }

//        /// <summary>
//        /// 重新连接
//        /// </summary>
//        /// <returns></returns>
//        public async ValueTask ReconnectAsync()
//        {
//            if (!AcquireReconnectLock())
//            {
//                return;
//            }

//            var newSocket = ReconnectGetNewSocket();

//            try
//            {
//                await newSocket.ConnectAsync(_serverHost!, _serverPort!.Value);

//                _socket.Close();
//                _socket = newSocket;
//            }
//            catch
//            {
//                newSocket.Close();
//            }
//            finally
//            {
//                _reconnecting = false;
//            }
//        }

//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        public virtual void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>
//        /// 获取重新连接锁，防止重复重连
//        /// </summary>
//        /// <returns></returns>
//        private bool AcquireReconnectLock()
//        {
//            lock (_reconnectLock)
//            {
//                if (_reconnecting || Connected || string.IsNullOrWhiteSpace(_serverHost) || _serverPort == null)
//                {
//                    return false;
//                }

//                _reconnecting = true;
//                return true;
//            }
//        }

//        /// <summary>
//        /// 重连获取新Socket
//        /// </summary>
//        /// <returns></returns>
//        private Socket ReconnectGetNewSocket()
//        {
//            var newSocket = new Socket(_socket.AddressFamily, _socket.SocketType, ProtocolType.Tcp);

//            if (_keepAlive)
//            {
//                newSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _keepAlive);
//            }

//            newSocket.SendBufferSize = _socket.SendBufferSize;
//            newSocket.ReceiveBufferSize = _socket.ReceiveBufferSize;
//            newSocket.SendTimeout = _socket.SendTimeout;
//            newSocket.ReceiveTimeout = _socket.ReceiveTimeout;

//            return newSocket;
//        }

//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        /// <param name="dispose"></param>
//        private void Dispose(bool dispose)
//        {
//            lock (_disposeLock)
//            {
//                if (_disposed)
//                {
//                    return;
//                }

//                if (dispose)
//                {
//                    _socket.Close();
//                }

//                _disposed = true;
//            }
//        }
//    }
//}