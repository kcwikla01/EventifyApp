using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SystemController : Controller
    {
        private readonly ISystemApplicationService _systemApplicationService;
        public SystemController(ISystemApplicationService systemApplicationService)
        {
            _systemApplicationService = systemApplicationService;
        }

        [HttpGet]
        public async Task<IActionResult> PingAnonymous()
        {
            return await _systemApplicationService.PingAnonymous();
        }
    }
}
