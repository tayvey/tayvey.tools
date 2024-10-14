using System.ServiceModel;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;

namespace Demo.WebApi.Business.Interfaces;

/// <summary>
/// SOAP DEMO接口
/// </summary>
[ServiceContract]
[TvAutoSoap(TvAutoSoapVersion.Soap12, "/soap/soapdemo.asmx")]
public interface ISoapDemo
{
    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    [OperationContract]
    public int GetSum(int a, int b);
}