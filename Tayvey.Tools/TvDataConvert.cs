using System.Net;

namespace Tayvey.Tools
{
    /// <summary>
    /// Tv数据转换
    /// </summary>
    public static class TvDataConvert
    {
        /// <summary>
        /// IP端点对象转换为IP地址+端口号元组对象
        /// </summary>
        /// <param name="endPoint">IP端点对象</param>
        /// <returns></returns>
        public static (string? ipAddress, ushort? port) TryParseToTuple(this IPEndPoint? endPoint)
        {
            try
            {
                if (endPoint == null)
                {
                    return (null, null);
                }

                var ipAddress = endPoint.Address.ToString();
                var port = (ushort)endPoint.Port;

                return (ipAddress, port);
            }
            catch
            {
                return (null, null);
            }
        }
    }
}