using System;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// �Զ�ע�ᶨʱ��������
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoCronJobAttribute : Attribute
    {
        /// <summary>
        /// CRON���ʽ
        /// </summary>
        internal readonly string _cron;

        /// <summary>
        /// ��ʶ����
        /// </summary>
        internal readonly string[] _marks;

        /// <summary>
        /// ��ʼ��
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