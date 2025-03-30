using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface ILoginApplicationService
    {
        Task<IActionResult> Login(UserDto userDto);
    }
}
