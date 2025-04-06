using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface ISystemApplicationService
    {
        Task<IActionResult> PingAnonymous();
    }
}
