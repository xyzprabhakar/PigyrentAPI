using API.Classes;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;

        public CommonController(ILogger<CommonController> logger)
        {
            _logger=logger;
        }
        [HttpGet]
        [Route("GetCaptcha")]
        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> GetCaptcha()
        {
            var code=Captcha.GenerateCaptchaCode();
            var data=Captcha.GenerateCaptchaImage(50,150, code);
            return Ok(data);
        }
    }
}
