using System;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// 自动注册SOAP特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoSoapAttribute : Attribute
    {
        /// <summary>
        /// SOAP版本
        /// </summary>
        internal readonly TvAutoSoapVersion _version;

        /// <summary>
        /// 地址
        /// </summary>
        internal readonly string _url;

        /// <summary>
        /// 标识数组
        /// </summary>
        internal readonly string[] _marks;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="version"></param>
        /// <param name="url"></param>
        /// <param name="marks"></param>
        public TvAutoSoapAttribute(TvAutoSoapVersion version, string url, params string[] marks)
        {
            _version = version;
            _url = url;
            _marks = marks;
        }
    }
}