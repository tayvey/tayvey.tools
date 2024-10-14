using Demo.WebApi.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controller
{
    /// <summary>
    /// DEMO
    /// </summary>
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        /// <summary>
        /// DEMO业务
        /// </summary>
        private readonly IDemo _demo;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="demo"></param>
        public DemoController(IDemo demo)
        {
            _demo = demo;
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSum(int a, int b)
        {
            return Ok(_demo.Sum(a, b));
        }
    }
}