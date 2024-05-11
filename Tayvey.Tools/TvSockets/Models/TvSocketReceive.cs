using System.Text;
using Tayvey.Tools.TvSockets.Enums;

namespace Tayvey.Tools.TvSockets.Models
{
    /// <summary>
    /// TvSocket服务端&客户端通用接收数据对象
    /// </summary>
    public class TvSocketReceive
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public TvSocketClient Client { get; }

        /// <summary>
        /// 接收数据字节长度
        /// </summary>
        public uint Length { get; }

        /// <summary>
        /// 接收数据字节数组
        /// </summary>
        public byte[] Data => Client.ReceiveBuffer;

        /// <summary>
        /// 接收数据字符串
        /// </summary>
        public string DataStr { get; }

        /// <summary>
        /// 粘包字符串
        /// </summary>
        public string DipBag => Client.DipBag;

        /// <summary>
        /// 数据来源身份
        /// </summary>
        public TvSocketRole DataSourceRole => Client.Role switch
        {
            TvSocketRole.Client => TvSocketRole.Server,
            _ => TvSocketRole.Client
        };

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="length">接收数据字节长度</param>
        public TvSocketReceive(TvSocketClient client, uint length)
        {
            Client = client;
            Length = length;
            DataStr = Encoding.UTF8.GetString(Data, 0, (int)Length);
        }
    }
}