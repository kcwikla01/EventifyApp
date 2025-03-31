using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly ILoginApplicationService _loginApplicationService;

        public AuthController(ILoginApplicationService loginApplicationService)
        {
            _loginApplicationService = loginApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            return await _loginApplicationService.Login(userDto);
        }
    }
}
