namespace Tayvey.Tools.TvSockets.Enums
{
    /// <summary>
    /// TvSocket角色
    /// </summary>
    public enum TvSocketRole
    {
        /// <summary>
        /// 客户端
        /// </summary>
        Client = 0,

        /// <summary>
        /// 服务端的子客户端
        /// </summary>
        ServerChildClient,

        /// <summary>
        /// 服务端
        /// </summary>
        Server
    }
}