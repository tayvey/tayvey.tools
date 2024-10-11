//#if NETSTANDARD2_1
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Threading;
//using System;
//using System.Linq;
//#endif

//namespace Tayvey.Tools
//{
//    /// <summary>
//    /// Tv异步
//    /// </summary>
//    public static class TvTasks
//    {
//        /// <summary>
//        /// Tv异步遍历
//        /// </summary>
//        /// <typeparam name="T">数据类型</typeparam>
//        /// <param name="iData">数据集合</param>
//        /// <param name="action">处理方法</param>
//        /// <param name="maxConcurrency">最大异步数量</param>
//        /// <param name="isGroup">是否分组遍历</param>
//        /// <returns></returns>
//        public static Task TvForEachAsync<T>(this IEnumerable<T>? iData, Action<T>? action, uint? maxConcurrency = null, bool isGroup = false)
//        {
//            if (iData == null || !iData.Any() || action == null)
//            {
//                return Task.CompletedTask;
//            }

//            if (maxConcurrency > 1 && isGroup)
//            {
//                return ForEachGroupAsync(iData, (item, index) => action.Invoke(item), maxConcurrency);
//            }
//            else
//            {
//                return ForEachAsync(iData, (item, index) => action.Invoke(item), maxConcurrency);
//            }
//        }

//        /// <summary>
//        /// Tv异步遍历
//        /// </summary>
//        /// <typeparam name="T">数据类型</typeparam>
//        /// <param name="iData">数据集合</param>
//        /// <param name="action">处理方法</param>
//        /// <param name="maxConcurrency">最大异步数量</param>
//        /// <param name="isGroup">是否分组遍历</param>
//        /// <returns></returns>
//        public static Task TvForEachAsync<T>(this IEnumerable<T>? iData, Action<T, uint>? action, uint? maxConcurrency = null, bool isGroup = false)
//        {
//            if (iData == null || !iData.Any() || action == null)
//            {
//                return Task.CompletedTask;
//            }

//            if (maxConcurrency > 1 && isGroup)
//            {
//                return ForEachGroupAsync(iData, action, maxConcurrency);
//            }
//            else
//            {
//                return ForEachAsync(iData, action, maxConcurrency);
//            }
//        }

//        /// <summary>
//        /// 异步分组遍历
//        /// </summary>
//        /// <typeparam name="T">数据类型</typeparam>
//        /// <param name="iData">数据集合</param>
//        /// <param name="action">处理方法</param>
//        /// <param name="maxConcurrency">最大异步数量</param>
//        /// <returns></returns>
//        private static async Task ForEachGroupAsync<T>(IEnumerable<T> iData, Action<T, uint> action, uint? maxConcurrency)
//        {
//            var iDataIndexPq = iData.Select((i, index) => new { item = i, index = (uint)index });
//            var iDataGroupPq = iDataIndexPq.GroupBy(i => i.index / maxConcurrency!.Value);

//            foreach (var group in iDataGroupPq)
//            {
//                var tasks = group.Select(groupItem => Task.Run(async () =>
//                {
//                    await Task.Run(() =>
//                    {
//                        action.Invoke(groupItem.item, groupItem.index);
//                    });
//                }));

//                await Task.WhenAll(tasks);
//            }
//        }

//        /// <summary>
//        /// 异步遍历
//        /// </summary>
//        /// <typeparam name="T">数据类型</typeparam>
//        /// <param name="iData">数据集合</param>
//        /// <param name="action">处理方法</param>
//        /// <param name="maxConcurrency">最大异步数量</param>
//        /// <returns></returns>
//        private static Task ForEachAsync<T>(IEnumerable<T> iData, Action<T, uint> action, uint? maxConcurrency)
//        {
//            var semaphore = maxConcurrency > 0 ? new SemaphoreSlim((int)maxConcurrency!.Value) : null;

//            var tasks = iData.Select((data, index) => Task.Run(async () =>
//            {
//                if (semaphore != null)
//                {
//                    await semaphore.WaitAsync();
//                }

//                await Task.Run(() =>
//                {
//                    action.Invoke(data, (uint)index);
//                });

//                semaphore?.Release();
//            }));

//            return Task.WhenAll(tasks);
//        }
//    }
//}