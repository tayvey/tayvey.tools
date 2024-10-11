//#if NETSTANDARD2_1
//using System.Security.Cryptography;
//using System.Text;
//#endif

//namespace Tayvey.Tools
//{
//    /// <summary>
//    /// Tv加密
//    /// </summary>
//    public static class TvEncryption
//    {
//        /// <summary>
//        /// MD5 32位加密
//        /// </summary>
//        /// <param name="plaintext">明文</param>
//        /// <param name="capital">是否大写</param>
//        /// <returns>密文</returns>
//        public static string MD5Encryption(this string plaintext, bool capital = false)
//        {
//            var inputBytes = Encoding.UTF8.GetBytes(plaintext);

//#if NET6_0_OR_GREATER
//            var hashBytes = MD5.HashData(inputBytes);
//#else
//            using MD5 md5 = MD5.Create();
//            var hashBytes = md5.ComputeHash(inputBytes);
//#endif

//            var sb = new StringBuilder();
//            foreach (var item in hashBytes)
//            {
//                sb.Append(item.ToString("x2"));
//            }

//            if (capital)
//            {
//                return sb.ToString().ToUpper();
//            }
//            else
//            {
//                return sb.ToString();
//            }
//        }
//    }
//}