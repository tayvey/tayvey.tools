#if NETSTANDARD2_1
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endif

namespace Tayvey.Tools.TvSockets
{
    /// <summary>
    /// TvTcp客户端
    /// </summary>
    public class TvTcpClient
    {
        /// <summary>
        /// Socket
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 保持连接
        /// </summary>
        private bool _keepAlive = false;

        /// <summary>
        /// 服务端地址
        /// </summary>
        private string? _serverHost;

        /// <summary>
        /// 服务端端口
        /// </summary>
        private int? _serverPort;

        /// <summary>
        /// 连接锁
        /// </summary>
#if NET6_0_OR_GREATER
        private readonly object _connectLock = new();
#else
        private readonly object _connectLock = new object();
#endif

        /// <summary>
        /// 是否已连接
        /// </summary>
        private bool _isConnected = false;

        /// <summary>
        /// 发送数据锁
        /// </summary>
#if NET6_0_OR_GREATER
        private readonly object _sendLock = new();
#else
        private readonly object _sendLock = new object();
#endif

        /// <summary>
        /// 是否正在发送数据
        /// </summary>
        private bool _isSending = false;

        /// <summary>
        /// 同步接收数据锁
        /// </summary>
#if NET6_0_OR_GREATER
        private readonly object _receiveLock = new();
#else
        private readonly object _receiveLock = new object();
#endif

        /// <summary>
        /// 是否正在接收数据
        /// </summary>
        private bool _isReceiving = false;

        /// <summary>
        /// 是否保持连接
        /// </summary>
        public bool IsKeepAlive
        {
            get
            {
                return _keepAlive;
            }
            set
            {
                _keepAlive = value;
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _keepAlive);
            }
        }

        /// <summary>
        /// 发送缓存区大小
        /// </summary>
        public int SendBufferSize
        {
            get => _socket.SendBufferSize;
            set => _socket.SendBufferSize = value < 1 ? 1 : value;
        }

        /// <summary>
        /// 接收缓存区大小
        /// </summary>
        public int ReceiveBufferSize
        {
            get => _socket.ReceiveBufferSize;
            set => _socket.ReceiveBufferSize = value < 1 ? 1 : value;
        }

        /// <summary>
        /// 是否已断开连接
        /// </summary>
        public bool IsDisconnected => _socket.Poll(1000, SelectMode.SelectError);

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        public TvTcpClient(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
        {
            _socket = new Socket(addressFamily, socketType, ProtocolType.Tcp);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connet(string host, int port)
        {
            lock (_connectLock)
            {
                if (_isConnected)
                {
                    return;
                }

                _socket.Connect(host, port);

                _isConnected = true;
                _serverHost = host;
                _serverPort = port;
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task ConnetAsync(string host, int port)
        {
            if (_isConnected)
            {
                return;
            }

            await _socket.ConnectAsync(host, port);

            _isConnected = true;
            _serverHost = host;
            _serverPort = port;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int Send(byte[] buffer)
        {
            if (!SendAcquireThreadLock())
            {
                return 0;
            }

            var send = _socket.Send(buffer);

            _isSending = false;

            return send;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public int Send(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var buffer = encoding.GetBytes(message);
            return Send(buffer);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public async Task<int> SendAsync(byte[] buffer)
        {
            if (!SendAcquireThreadLock())
            {
                return 0;
            }

#if NET6_0_OR_GREATER
            var send = await _socket.SendAsync(new ReadOnlyMemory<byte>(buffer), SocketFlags.None, default);

            _isSending = false;

            return send;
#else
            var tcs = new TaskCompletionSource<int>();
            var sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(buffer, 0, buffer.Length);
            sendArgs.Completed += (s, e) =>
            {
                if (e.SocketError == SocketError.Success)
                {
                    tcs.SetResult(e.BytesTransferred);
                }
                else
                {
                    tcs.SetException(new SocketException((int)e.SocketError));
                }
            };

            if (!_socket.SendAsync(sendArgs))
            {
                if (sendArgs.SocketError == SocketError.Success)
                {
                    tcs.SetResult(sendArgs.BytesTransferred);
                }
                else
                {
                    tcs.SetException(new SocketException((int)sendArgs.SocketError));
                }
            }

            var send = await tcs.Task;

            _isSending = false;

            return send;
#endif
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Task<int> SendAsync(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var buffer = encoding.GetBytes(message);
            return SendAsync(buffer);
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        public byte[] Receive()
        {
            if (!ReceiveAcquireThreadLock())
            {
#if NET8_0_OR_GREATER
                return [];
#else
                return Array.Empty<byte>();
#endif
            }

            var buffer = new byte[_socket.Available];
            _socket.Receive(buffer);

            _isReceiving = false;

            return buffer;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="escape"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Receive(bool escape, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var buffer = Receive();
            if (buffer.Length == 0)
            {
                return "";
            }

            var str = encoding.GetString(buffer);

            if (escape)
            {
                return Regex.Escape(str);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReceiveAsync()
        {
            if (!ReceiveAcquireThreadLock())
            {
#if NET8_0_OR_GREATER
                return [];
#else
                return Array.Empty<byte>();
#endif
            }

            var buffer = new byte[_socket.Available];

#if NET6_0_OR_GREATER
            await _socket.ReceiveAsync(buffer, SocketFlags.None, default);
#else
            var tcs = new TaskCompletionSource<int>();
            var sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(buffer, 0, buffer.Length);
            sendArgs.Completed += (s, e) =>
            {
                if (e.SocketError == SocketError.Success)
                {
                    tcs.SetResult(e.BytesTransferred);
                }
                else
                {
                    tcs.SetException(new SocketException((int)e.SocketError));
                }
            };

            if (!_socket.ReceiveAsync(sendArgs))
            {
                if (sendArgs.SocketError == SocketError.Success)
                {
                    tcs.SetResult(sendArgs.BytesTransferred);
                }
                else
                {
                    tcs.SetException(new SocketException((int)sendArgs.SocketError));
                }
            }

            await tcs.Task;
#endif
            _isReceiving = false;

            return buffer;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="escape"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<string> ReceiveAsync(bool escape, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var buffer = await ReceiveAsync();
            if (buffer.Length == 0)
            {
                return "";
            }

            var str = encoding.GetString(buffer);

            if (escape)
            {
                return Regex.Escape(str);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 获取发送数据线程锁
        /// </summary>
        /// <returns></returns>
        private bool SendAcquireThreadLock()
        {
            lock (_sendLock)
            {
                while (_isSending)
                {
                }

                if (!_socket.Poll(1000, SelectMode.SelectWrite))
                {
                    return false;
                }
                else
                {
                    _isSending = true;
                    return true;
                }
            }
        }

        /// <summary>
        /// 获取接收数据线程锁
        /// </summary>
        /// <returns></returns>
        private bool ReceiveAcquireThreadLock()
        {
            lock (_receiveLock)
            {
                if (_isReceiving || _socket.Available == 0)
                {
                    return false;
                }
                else
                {
                    _isReceiving = true;
                    return true;
                }
            }
        }
    }
}