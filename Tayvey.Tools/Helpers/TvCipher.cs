using System;
using System.Security.Cryptography;
using System.Text;

namespace Tayvey.Tools
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
        public static string TvToMD5(this string str)
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

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TvBase64Decoding(this string str)
        {
            var buffer = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}