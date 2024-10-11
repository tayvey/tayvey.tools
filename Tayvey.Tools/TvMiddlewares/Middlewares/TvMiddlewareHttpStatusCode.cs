//#if NETSTANDARD2_1
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System.Threading.Tasks;
//using Tayvey.Tools.TvApiResults.Models;
//#endif

//namespace Tayvey.Tools.TvMiddlewares.Middlewares
//{
//    /// <summary>
//    /// Tv中间件Http状态码
//    /// </summary>
//#if NET8_0_OR_GREATER
//    /// <param name="next">请求委托</param>
//    public abstract class TvMiddlewareHttpStatusCode(RequestDelegate next) : TvMiddlewareBase(next)
//#else
//    public abstract class TvMiddlewareHttpStatusCode : TvMiddlewareBase
//#endif
//    {
//#if NET6_0 || NETSTANDARD2_1
//        /// <summary>
//        /// 初始化构造
//        /// </summary>
//        /// <param name="next">请求委托</param>
//        public TvMiddlewareHttpStatusCode(RequestDelegate next) : base(next)
//        {
//        }
//#endif

//        /// <summary>
//        /// 异步调用
//        /// </summary>
//        /// <param name="context">请求上下文</param>
//        /// <returns></returns>
//        public override async Task InvokeAsync(HttpContext context)
//        {
//            // 调用下一个中间件
//            await Next.Invoke(context);

//            // 非TvApiResult返回才做处理
//            if (!context.Response.Headers.ContainsKey("Tv-Api-Result"))
//            {
//                // 自定义处理
//                if (!await CustomReturn(context))
//                {
//                    await ReturnHttpStatusCode(context);
//                }
//            }
//        }

//        /// <summary>
//        /// 返回HTTP状态码处理
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        private static async Task ReturnHttpStatusCode(HttpContext context)
//        {
//            // 返回对象
//            var result = context.Response.StatusCode switch
//            {
//                404 => TvApiResult.NotFound("请求资源不存在"),
//                405 => TvApiResult.MethodNotAllowed("方法不被允许"),
//                _ => null
//            };

//            // 未处理的状态码
//            if (result == null)
//            {
//                return;
//            }

//            var response = context.Response;
//            response.ContentType = "application/json; charset=utf-8";

//#if NET6_0_OR_GREATER
//            response.Headers.Server = "";
//            response.Headers.Append("Tv-Api-Result", "true");
//#else
//            response.Headers["Server"] = "";
//            response.Headers["Tv-Api-Result"] = "true";
//#endif

//            response.StatusCode = result.StatusCode.GetHashCode();

//            await response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver()
//            }));
//        }

//        /// <summary>
//        /// 自定义返回
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        protected abstract Task<bool> CustomReturn(HttpContext context);
//    }
//}