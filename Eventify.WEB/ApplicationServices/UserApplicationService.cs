using Eventify.Database.DbContext;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using AutoMapper;
using Eventify.UoW.Base;

namespace Eventify.WEB.ApplicationServices
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IManageUsersUoW _manageUsersUoW;
        private readonly IMapper _mapper;
        public UserApplicationService(IManageUsersUoW manageUsersUoW, IMapper mapper)
        {
            _manageUsersUoW = manageUsersUoW;
            _mapper = mapper;
        }

        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            if (!IsValidEmail(userDto.Email))
            {
                return new BadRequestObjectResult("Invalid email format.");
            }

            bool userExist = await _manageUsersUoW.CheckIfUserExist(userDto);
            
            if (userExist)
            {
                return new BadRequestObjectResult("User with this email or name exist");
            }

            var newUser = _mapper.Map<UserDto>(await _manageUsersUoW.CreateUser(userDto));
            return new OkObjectResult(newUser);
        }

        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _manageUsersUoW.GetUserById(id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not exist");
            }

            return new OkObjectResult(_mapper.Map<UserDto>(user));
        }

        public async Task<IActionResult> ChangePassword(UserDto userDto, string newPassword)
        {
            bool userExist = await _manageUsersUoW.CheckIfUserExist(userDto);

            if (!userExist)
            {
                return new NotFoundObjectResult("User with this email or name not exist");
            }
            
            bool isChanged =  await _manageUsersUoW.ChangePassword(userDto, newPassword);

           if (isChanged)
           {
               userDto.Password = newPassword;
               return new OkObjectResult(userDto);
           }

           return new NotFoundObjectResult(
               "Invalid username or password. Please try again.");
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
}
