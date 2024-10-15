using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tayvey.Tools.Helpers
{
    /// <summary>
    /// TvTcp服务端
    /// </summary>
    public class TvTcpServer : IDisposable
    {
        /// <summary>
        /// Socket
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// 释放锁
        /// </summary>
        private readonly object _disposeLock = new object();

        /// <summary>
        /// 已释放
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 保持连接
        /// </summary>
        private bool _keepAlive = false;

        /// <summary>
        /// 保持连接
        /// </summary>
        public bool KeepAlive
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
        /// 接收超时（毫秒）
        /// </summary>
        public int ReceiveTimeout
        {
            get => _socket.ReceiveTimeout;
            set => _socket.ReceiveTimeout = value;
        }

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        public TvTcpServer(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
        {
            _socket = new Socket(addressFamily, socketType, ProtocolType.Tcp);
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~TvTcpServer()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 绑定并监听
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="backlog"></param>
        public void BindAndListen(string host, int port, int backlog)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            _socket.Listen(backlog);
        }

        /// <summary>
        /// 接收连接
        /// </summary>
        /// <returns></returns>
        public TvTcpClient Accept() => new TvTcpClient(_socket.Accept())
        {
            KeepAlive = _keepAlive,
            SendBufferSize = _socket.SendBufferSize,
            ReceiveBufferSize = _socket.ReceiveBufferSize,
            SendTimeout = _socket.SendTimeout,
            ReceiveTimeout = _socket.ReceiveTimeout
        };

        /// <summary>
        /// 接收连接
        /// </summary>
        /// <returns></returns>
        public async Task<TvTcpClient> AcceptAsync() => new TvTcpClient(await _socket.AcceptAsync())
        {
            KeepAlive = _keepAlive,
            SendBufferSize = _socket.SendBufferSize,
            ReceiveBufferSize = _socket.ReceiveBufferSize,
            SendTimeout = _socket.SendTimeout,
            ReceiveTimeout = _socket.ReceiveTimeout
        };

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="dispose"></param>
        private void Dispose(bool dispose)
        {
            lock (_disposeLock)
            {
                if (_disposed)
                {
                    return;
                }

                if (dispose)
                {
                    _socket.Close();
                }

                _disposed = true;
            }
        }
    }
}