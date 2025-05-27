using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IUserApplicationService
    {
        Task<IActionResult> CreateUser(UserDto userDto);
        Task<IActionResult> GetUserById(int id);
        Task<IActionResult> ChangePassword(UserDto userDto, string newPassword, int userId);
    }
}
