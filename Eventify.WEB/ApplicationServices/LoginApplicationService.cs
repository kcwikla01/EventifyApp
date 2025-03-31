using AutoMapper;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class LoginApplicationService : ILoginApplicationService
    {
        private readonly IManageUsersUoW _manageUsersUoW;
        private readonly IManageRolesUoW _manageRoleUoW;
        private readonly IMapper _mapper;
        public LoginApplicationService(IManageUsersUoW manageUsersUoW, IManageRolesUoW manageRoleUoW, IMapper mapper)
        {
            _manageUsersUoW = manageUsersUoW;
            _manageRoleUoW = manageRoleUoW;
            _mapper = mapper;
        }
        public async Task<IActionResult> Login(UserDto userDto)
        {
            bool userExist = await _manageUsersUoW.CheckIfUserExist(userDto);
            if (!userExist)
            {
                return new NotFoundObjectResult("User with this email or name not exist");
            }

            var userPasswordCorrect = await _manageUsersUoW.CheckIfUserPasswordCorrect(userDto);
            
            if(userPasswordCorrect == null)
            {
                return new BadRequestObjectResult("Password is incorrect");
            }

            var role =  await _manageRoleUoW.GetRoleName(userPasswordCorrect.RoleId);

            if (role == null)
            {
                return new NotFoundObjectResult("Role not exist");
            }

            var loginDto = _mapper.Map<LoginDto>(role);
            loginDto.UserId = userPasswordCorrect.Id;

            return new OkObjectResult(_mapper.Map<LoginDto>(role));
        }
    }
}
