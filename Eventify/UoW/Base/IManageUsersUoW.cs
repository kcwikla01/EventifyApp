using Eventify.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageUsersUoW
    {
        Task<User> CreateUser(UserDto userDto);
        Task<int> GetUserByName(string userName);
        Task<User> GetUserById(int id);
        Task<bool> CheckIfUserExist(UserDto user);
        Task<bool> ChangePassword(UserDto userDto, string newPassword);
        Task<User> CheckIfUserPasswordCorrect(UserDto userDto);
    }
}
