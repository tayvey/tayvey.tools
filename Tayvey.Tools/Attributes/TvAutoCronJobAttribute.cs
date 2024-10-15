using System;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// 自动注册定时任务特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoCronJobAttribute : Attribute
    {
        /// <summary>
        /// CRON表达式
        /// </summary>
        internal readonly string _cron;

        /// <summary>
        /// 标识数组
        /// </summary>
        internal readonly string[] _marks;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cron"></param>
        /// <param name="marks"></param>
        public TvAutoCronJobAttribute(string cron, params string[] marks)
        {
            _cron = cron;
            _marks = marks;
        }
    }
}