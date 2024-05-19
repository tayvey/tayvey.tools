#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
using System;
using System.Net.Sockets;
#endif

namespace Tayvey.Tools.TvSockets.Models
{
    /// <summary>
    /// TvSocket服务端&客户端通用日志对象
    /// </summary>
    public class TvSocketLog
    {
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 连接协议
        /// </summary>
        public ProtocolType ProtoType { get; }

        /// <summary>
        /// 服务端IP地址
        /// </summary>
        public string? ServerIpAddress { get; }

        /// <summary>
        /// 服务端端口号
        /// </summary>
        public ushort? ServerPort { get; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
#if NET6_0_OR_GREATER
        public string? ClientIpAddress { get; init; }
#elif NETSTANDARD2_1
        public string? ClientIpAddress { get; }
#endif

        /// <summary>
        /// 客户端端口号
        /// </summary>
#if NET6_0_OR_GREATER
        public ushort? ClientPort { get; init; }
#elif NETSTANDARD2_1
        public ushort? ClientPort { get; }
#endif

        /// <summary>
        /// 异常
        /// </summary>
#if NET6_0_OR_GREATER
        public Exception? Ex { get; init; }
#elif NETSTANDARD2_1
        public Exception? Ex { get; }
#endif

        /// <summary>
        /// 服务端日志初始化构造
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="server">服务端</param>
#if NET6_0_OR_GREATER
        public TvSocketLog(string text, TvSocketServer server)
#elif NETSTANDARD2_1
        /// <param name="clientIpAddress">客户端IP地址</param>
        /// <param name="clientPort">客户端端口号</param>
        /// <param name="ex">异常</param>
        public TvSocketLog(
            string text,
            TvSocketServer server,
            string? clientIpAddress = null,
            ushort? clientPort = null,
            Exception? ex = null
        )
#endif
        {
            Text = text;
            ProtoType = server.ProtoType;
            ServerIpAddress = server.ServerIpAddress;
            ServerPort = server.ServerPort;

#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
            ClientIpAddress = clientIpAddress;
            ClientPort = clientPort;
            Ex = ex;
#endif
        }

        /// <summary>
        /// 客户端日志初始化构造
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="client">客户端</param>
#if NET6_0_OR_GREATER
        public TvSocketLog(string text, TvSocketClient client)
#elif NETSTANDARD2_1
        /// <param name="ex">异常</param>
        public TvSocketLog(string text, TvSocketClient client, Exception? ex = null)
#endif
        {
            Text = text;
            ProtoType = client.ProtoType;
            ServerIpAddress = client.ServerIpAddress;
            ServerPort = client.ServerPort;
            ClientIpAddress = client.ClientIpAddress;
            ClientPort = client.ClientPort;

#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
            Ex = ex;
#endif
        }
    }
}