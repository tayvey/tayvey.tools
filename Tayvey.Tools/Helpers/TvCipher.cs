using System.Security.Cryptography;
using System.Text;

namespace Tayvey.Tools.Helpers
{
    /// <summary>
    /// 密钥
    /// </summary>
    public static class TvCipher
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);

            using MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var item in hash)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}