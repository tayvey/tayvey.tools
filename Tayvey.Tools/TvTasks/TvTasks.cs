using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tayvey.Tools.TvTasks.Models;

namespace Tayvey.Tools.TvTasks
{
    /// <summary>
    /// Tv异步
    /// </summary>
    public static class TvTasks
    {
        /// <summary>
        /// Tv异步遍历
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="options">异步遍历选项</param>
        /// <returns></returns>
        public static Task TvForEachAsync<T>(this IEnumerable<T>? iData, TvForEachOptions<T> options)
        {
            if (iData == null || !iData.Any())
            {
                return Task.CompletedTask;
            }

            if (options.MaxConcurrency > 1 && options.IsGroup)
            {
                return ForEachGroupAsync(iData, options);
            }
            else
            {
                return ForEachAsync(iData, options);
            }
        }

        /// <summary>
        /// 异步分组遍历
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="options">异步遍历选项</param>
        /// <returns></returns>
        private static async Task ForEachGroupAsync<T>(IEnumerable<T> iData, TvForEachOptions<T> options)
        {
            var iDataPq = iData.AsParallel();
            var iDataIndexPq = iDataPq.Select((i, index) => new { item = i, index = (uint)index });
            var iDataGroupPq = iDataIndexPq.GroupBy(i => i.index / options.MaxConcurrency!.Value);

            foreach (var group in iDataGroupPq)
            {
                var groupPq = group.AsParallel();

                var tasks = groupPq.Select(groupItem => Task.Run(async () =>
                {
                    var tvTaskItem = new TvForEachItem<T>(groupItem.item, groupItem.index);

                    try
                    {
                        if (options.TryAction != null)
                        {
                            await Task.Run(() => options.TryAction?.Invoke(tvTaskItem));
                        }
                    }
                    catch (Exception e)
                    {
                        if (options.CatchAction != null)
                        {
                            await Task.Run(() => options.CatchAction?.Invoke(tvTaskItem, e));
                        }
                    }
                }));

                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 异步遍历
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="options">异步遍历选项</param>
        /// <returns></returns>
        private static Task ForEachAsync<T>(IEnumerable<T> iData, TvForEachOptions<T> options)
        {
            var semaphore = options.MaxConcurrency > 0 ? new SemaphoreSlim((int)options.MaxConcurrency!.Value) : null;

            var iDataPq = iData.AsParallel();

            var tasks = iDataPq.Select((data, index) => Task.Run(async () =>
            {
                var tvTaskItem = new TvForEachItem<T>(data, (uint)index);

                try
                {
                    if (semaphore != null)
                    {
                        await semaphore.WaitAsync();
                    }

                    if (options.TryAction != null)
                    {
                        await Task.Run(() => options.TryAction?.Invoke(tvTaskItem));
                    }

                    options.TryAction?.Invoke(tvTaskItem);
                }
                catch (Exception e)
                {
                    if (options.CatchAction != null)
                    {
                        await Task.Run(() => options.CatchAction?.Invoke(tvTaskItem, e));
                    }

                    options.CatchAction?.Invoke(tvTaskItem, e);
                }
                finally
                {
                    semaphore?.Release();
                }
            }));

            return Task.WhenAll(tasks);
        }
    }
}
