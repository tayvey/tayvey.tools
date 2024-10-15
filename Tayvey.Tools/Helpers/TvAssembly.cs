using System;
using System.Linq;

namespace Tayvey.Tools.Helpers
{
    /// <summary>
    /// 程序集
    /// </summary>
    internal static class TvAssembly
    {
        /// <summary>
        /// 获取已加载的程序集
        /// </summary>
        /// <returns></returns>
        internal static Type[] GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(i => i.GetTypes())
                .SelectMany(i => i).ToArray();
        }
    }
}