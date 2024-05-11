using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tayvey.Tools.TvTasks.Models;

namespace Tayvey.Tools.TvTasks
{
    /// <summary>
    /// Tv异步任务
    /// </summary>
    public static class TvTasks
    {
        /// <summary>
        /// 异步遍历
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="tryAction">数据处理</param>
        /// <param name="catchAction">异常处理</param>
        /// <param name="maxConcurrency">最大任务执行量</param>
        /// <param name="groupExec">分组执行 (最大任务执行量必须 > 1)</param>
        public static void TvForEach<T>(
            this IEnumerable<T>? iData,
            Action<TvTaskItem<T>>? tryAction,
            Action<TvTaskItem<T>, Exception>? catchAction = null,
            uint? maxConcurrency = null,
            bool groupExec = false
        )
        {
            if (iData == null || !iData.Any())
            {
                return;
            }

            if (maxConcurrency > 1 && groupExec)
            {
                GroupExec(iData, maxConcurrency!.Value, tryAction, catchAction);
            }
            else
            {
                Exec(iData, maxConcurrency, tryAction, catchAction);
            }
        }

        /// <summary>
        /// 分组执行任务
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="maxConcurrency">每组数据量 (最大任务执行量)</param>
        /// <param name="tryAction">数据处理</param>
        /// <param name="catchAction">异常处理</param>
        private static void GroupExec<T>(
            IEnumerable<T> iData,
            uint maxConcurrency,
            Action<TvTaskItem<T>>? tryAction,
            Action<TvTaskItem<T>, Exception>? catchAction
        )
        {
            var iDataPq = iData.AsParallel();
            var iDataIndexPq = iDataPq.Select((i, index) => new { item = i, index = (uint)index });
            var iDataGroupPq = iDataIndexPq.GroupBy(i => i.index / maxConcurrency);

            foreach (var group in iDataGroupPq)
            {
                var groupPq = group.AsParallel();
                var tasks = groupPq.Select(groupItem => Task.Run(() =>
                {
                    var tvTaskItem = new TvTaskItem<T>(groupItem.item, groupItem.index);

                    try
                    {
                        tryAction?.Invoke(tvTaskItem);
                    }
                    catch (Exception e)
                    {
                        catchAction?.Invoke(tvTaskItem, e);
                    }
                }));
                Task.WaitAll(tasks.ToArray());
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="maxConcurrency">最大任务执行量</param>
        /// <param name="tryFunc">数据处理</param>
        /// <param name="catchFunc">异常处理</param>
        private static void Exec<T>(
            IEnumerable<T> iData,
            uint? maxConcurrency,
            Action<TvTaskItem<T>>? tryFunc,
            Action<TvTaskItem<T>, Exception>? catchFunc
        )
        {
            var semaphore = maxConcurrency > 0 ? new SemaphoreSlim((int)maxConcurrency!.Value) : null;

            var iDataPq = iData.AsParallel();
            var tasks = iDataPq.Select((data, index) => Task.Run(() =>
            {
                var tvTaskItem = new TvTaskItem<T>(data, (uint)index);

                try
                {
                    semaphore?.Wait();
                    tryFunc?.Invoke(tvTaskItem);
                }
                catch (Exception e)
                {
                    catchFunc?.Invoke(tvTaskItem, e);
                }
                finally
                {
                    semaphore?.Release();
                }
            }));
            Task.WaitAll(tasks.ToArray());
        }

        public static Task TvForEachAsync<T>(
            this IEnumerable<T>? iData,
            Func<TvTaskItem<T>, Task>? tryFunc,
            Func<TvTaskItem<T>, Exception, Task>? catchFunc = null,
            uint? maxConcurrency = null,
            bool groupExec = false
        )
        {
            if (iData == null || !iData.Any())
            {
                return Task.CompletedTask;
            }

            if (maxConcurrency > 1 && groupExec)
            {
                return GroupExecAsync(iData, maxConcurrency!.Value, tryFunc, catchFunc);
            }
            else
            {
                return ExecAsync(iData, maxConcurrency, tryFunc, catchFunc);
            }
        }

        /// <summary>
        /// 分组执行任务 (异步)
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="maxConcurrency">每组数据量 (最大任务执行量)</param>
        /// <param name="tryFunc">数据处理</param>
        /// <param name="catchFunc">异常处理</param>
        private static async Task GroupExecAsync<T>(
            IEnumerable<T> iData,
            uint maxConcurrency,
            Func<TvTaskItem<T>, Task>? tryFunc,
            Func<TvTaskItem<T>, Exception, Task>? catchFunc
        )
        {
            var iDataPq = iData.AsParallel();
            var iDataIndexPq = iDataPq.Select((i, index) => new { item = i, index = (uint)index });
            var iDataGroupPq = iDataIndexPq.GroupBy(i => i.index / maxConcurrency);

            foreach (var group in iDataGroupPq)
            {
                var groupPq = group.AsParallel();
                var tasks = groupPq.Select(groupItem => Task.Run(async () =>
                {
                    var tvTaskItem = new TvTaskItem<T>(groupItem.item, groupItem.index);

                    try
                    {
                        if (tryFunc != null)
                        {
                            await tryFunc.Invoke(tvTaskItem);
                        }
                    }
                    catch (Exception e)
                    {
                        if (catchFunc != null)
                        {
                            await catchFunc.Invoke(tvTaskItem, e);
                        }
                    }
                }));
                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 执行任务 (异步)
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="iData">数据集合</param>
        /// <param name="maxConcurrency">最大任务执行量</param>
        /// <param name="tryFunc">数据处理</param>
        /// <param name="catchFunc">异常处理</param>
        private static Task ExecAsync<T>(
            IEnumerable<T> iData,
            uint? maxConcurrency,
            Func<TvTaskItem<T>, Task>? tryFunc,
            Func<TvTaskItem<T>, Exception, Task>? catchFunc
        )
        {
            var semaphore = maxConcurrency > 0 ? new SemaphoreSlim((int)maxConcurrency!.Value) : null;

            var iDataPq = iData.AsParallel();
            var tasks = iDataPq.Select((data, index) => Task.Run(async () =>
            {
                var tvTaskItem = new TvTaskItem<T>(data, (uint)index);

                try
                {
                    if (semaphore != null)
                    {
                        await semaphore.WaitAsync();
                    }

                    if (tryFunc != null)
                    {
                        await tryFunc.Invoke(tvTaskItem);
                    }
                }
                catch (Exception e)
                {
                    if (catchFunc != null)
                    {
                        await catchFunc.Invoke(tvTaskItem, e);
                    }
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
