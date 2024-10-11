using System;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// �Զ�����ע������
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoDIAttribute : Attribute
    {
        internal readonly TvAutoDILifeCycle _lifeCycle;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="lifeCycle"></param>
        public TvAutoDIAttribute(TvAutoDILifeCycle lifeCycle)
        {
            _lifeCycle = lifeCycle;
        }
    }
}