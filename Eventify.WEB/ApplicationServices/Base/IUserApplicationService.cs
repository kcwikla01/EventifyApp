using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IUserApplicationService
    {
        public IActionResult CreateUser(UserDto userDto);
    }
}
