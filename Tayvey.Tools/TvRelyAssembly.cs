#if NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
#endif

namespace Tayvey.Tools
{
    /// <summary>
    /// Tv依赖程序集
    /// </summary>
    internal static partial class TvRelyAssembly
    {
        /// <summary>
        /// 依赖Tayvey.Tools的程序集列表
        /// </summary>
#if NET6_0_OR_GREATER
        internal static readonly Lazy<List<Assembly>> RelyAssemblies = new(LzRelyAssemblies);
#else
        internal static readonly Lazy<List<Assembly>> RelyAssemblies = new Lazy<List<Assembly>>(LzRelyAssemblies);
#endif

        /// <summary>
        /// 依赖Tayvey.Tools的程序集列表中的所有类型
        /// </summary>
#if NET6_0_OR_GREATER
        internal static readonly Lazy<List<Type>> LzRelyAssemblyTypes = new(
#else
        internal static readonly Lazy<List<Type>> LzRelyAssemblyTypes = new Lazy<List<Type>>(
#endif
            RelyAssemblies.Value.AsParallel().Select(i => i.GetTypes()).SelectMany(i => i).ToList()
        );

        /// <summary>
        /// 懒加载依赖Tayvey.Tools的程序集列表
        /// </summary>
        /// <returns></returns>
        private static List<Assembly> LzRelyAssemblies()
        {
            // 获取入口程序集
            var entry = Assembly.GetEntryAssembly();

            // 获取入口程序集依赖项文件
            var file = Path.Combine(AppContext.BaseDirectory, $"{entry?.GetName().Name}.deps.json");
            if (!File.Exists(file))
            {
#if NET8_0_OR_GREATER
                return [];
#elif NET6_0_OR_GREATER
                return new();
#else
                return new List<Assembly>();
#endif
            }

            // 读取入口程序集依赖项文件内容
            var content = File.ReadAllText(file);
#if NET8_0_OR_GREATER
            content = ContentCompress().Replace(content, "");
#else
            content = Regex.Replace(content, @"([ ]|\r\n|\n)+", "");
#endif

            // 递归获取依赖程序集名称列表
            var assemblyNames = GetRelyAssemblyNames(content);

            // 获取已加载的程序集
            var loadAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 返回依赖程序集
            return assemblyNames.AsParallel().Select(i =>
            {
                var assembly = loadAssemblies.FirstOrDefault(x => x.GetName().Name == i);

                if (assembly != null)
                {
                    return assembly;
                }

                return Assembly.Load(i);
            }).ToList();
        }

        /// <summary>
        /// 递归获取依赖程序集名称列表
        /// </summary>
        /// <param name="content">入口程序集依赖项文件内容</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        private static HashSet<string> GetRelyAssemblyNames(string content, string assemblyName = "Tayvey.Tools")
        {
            // 生成正则表达式
            var regex = new Regex(@$"(?<=""dependencies"":{{""{assemblyName}"":""[^""]+""}},""runtime"":{{"")[^""]+(?=[.]dll"")");

            // 获取匹配项
            var assemblyNames = new List<string>();
            foreach (Match item in regex.Matches(content))
            {
                assemblyNames.Add(item.Value);

                // 递归
                assemblyNames.AddRange(GetRelyAssemblyNames(content, item.Value));
            }
#if NET8_0_OR_GREATER
            return [.. assemblyNames];
#else
            return assemblyNames.ToHashSet();
#endif
        }

#if NET8_0_OR_GREATER
        /// <summary>
        /// 入口程序集依赖项文件压缩
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"([ ]|\r\n|\n)+")]
        private static partial Regex ContentCompress();
#endif
    }
}