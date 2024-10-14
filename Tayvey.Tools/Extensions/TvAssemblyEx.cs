using System;
using System.Linq;

namespace Tayvey.Tools.Extensions
{
    /// <summary>
    /// 程序集扩展
    /// </summary>
    internal static class TvAssemblyEx
    {
        /// <summary>
        /// 已加载的程序集
        /// </summary>
        internal static readonly Type[] LoadedTypes = GetLoadedAssemblies();

        /// <summary>
        /// 获取已加载的程序集
        /// </summary>
        /// <returns></returns>
        private static Type[] GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(i => i.GetTypes())
                .SelectMany(i => i).ToArray();
        }
    }
}