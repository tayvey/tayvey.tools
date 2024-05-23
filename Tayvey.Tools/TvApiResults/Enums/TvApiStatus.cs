namespace Tayvey.Tools.TvApiResults.Enums
{
    /// <summary>
    /// TvApi状态码
    /// </summary>
    public enum TvApiStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 200,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 400,

        /// <summary>
        /// 鉴权失败
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 资源不存在
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 方法不被允许
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// 异常
        /// </summary>
        Error = 500
    }
}