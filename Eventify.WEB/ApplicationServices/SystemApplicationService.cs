using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class SystemApplicationService : ISystemApplicationService
    {
        public async Task<IActionResult> PingAnonymous()
        {
            return new OkObjectResult("Service available");
        }
    }
}
