#if NETSTANDARD2_1
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Tayvey.Tools.TvSockets
{
    /// <summary>
    /// TvSocket
    /// </summary>
    public class TvSocket
    {
        /// <summary>
        /// Socket
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 远程主机地址
        /// </summary>
        private string? _remoteHost;

        /// <summary>
        /// 远程主机端口
        /// </summary>
        private int? _remotePort;

        /// <summary>
        /// 远程主机连接锁
        /// </summary>
#if NET6_0_OR_GREATER
        private readonly object _remoteConnectLock = new();
#else
        private readonly object _remoteConnectLock = new object();
#endif

        /// <summary>
        /// 远程主机是否已连接
        /// </summary>
        private bool _remoteIsConnected = false;

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
        private bool _isReceive = false;

        /// <summary>
        /// 发送缓存区大小
        /// </summary>
        public int SendBufferSize
        {
            get => _socket.SendBufferSize;
            set => _socket.SendBufferSize = value;
        }

        /// <summary>
        /// 接收缓存区大小
        /// </summary>
        public int ReceiveBufferSize
        {
            get => _socket.ReceiveBufferSize;
            set => _socket.ReceiveBufferSize = value;
        }

        /// <summary>
        /// 发送超时（毫秒）
        /// </summary>
        public int SendTimeout
        {
            get => _socket.SendTimeout;
            set => _socket.SendTimeout = value;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="protocolType"></param>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        public TvSocket(ProtocolType protocolType, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
        {
            _socket = new Socket(addressFamily, socketType, protocolType);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connet(string host, int port)
        {
            lock (_remoteConnectLock)
            {
                if (_remoteIsConnected)
                {
                    return;
                }

                _socket.Connect(host, port);

                _remoteIsConnected = true;
                _remoteHost = host;
                _remotePort = port;
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
            if (_remoteIsConnected)
            {
                return;
            }

            await _socket.ConnectAsync(host, port);

            _remoteIsConnected = true;
            _remoteHost = host;
            _remotePort = port;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int Send(byte[] buffer) => _socket.Send(buffer);

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
        /// <exception cref="SocketException"></exception>
        public async Task<int> SendAsync(byte[] buffer)
        {
            using var cancel = new CancellationTokenSource();
            if (SendTimeout > 0)
            {
                cancel.CancelAfter(TimeSpan.FromMilliseconds(SendTimeout));
            }

#if NET6_0_OR_GREATER
            try
            {
                return await _socket.SendAsync(new ReadOnlyMemory<byte>(buffer), SocketFlags.None, cancel.Token).AsTask();
            }
            catch (OperationCanceledException)
            {
                throw new SocketException(10060);
            }
#else
            var tcs = new TaskCompletionSource<int>();
            cancel.Token.Register(() => tcs.TrySetCanceled(cancel.Token));

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

            try
            {
                return await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                throw new SocketException(10060);
            }
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

            var buffer = new byte[_socket.ReceiveBufferSize];
            var receive = _socket.Receive(buffer);

            _isReceive = false;

            return buffer.Take(receive).ToArray();
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="escape"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Receive(bool escape, Encoding ? encoding = null)
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

            var buffer = new byte[_socket.ReceiveBufferSize];

#if NET6_0_OR_GREATER
            var receive = await _socket.ReceiveAsync(buffer, SocketFlags.None, default);
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

            var receive = await tcs.Task;
#endif
            _isReceive = false;

            return buffer.Take(receive).ToArray();
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
        /// 获取接收数据线程锁
        /// </summary>
        /// <returns></returns>
        private bool ReceiveAcquireThreadLock()
        {
            lock (_receiveLock)
            {
                if (_isReceive || _socket.Available == 0)
                {
                    return false;
                }
                else
                {
                    _isReceive = true;
                    return true;
                }
            }
        }
    }
}