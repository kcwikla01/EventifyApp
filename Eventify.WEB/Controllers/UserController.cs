﻿using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserApplicationService _userApplicationService;
        public UserController(IUserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            return await _userApplicationService.CreateUser(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            return await _userApplicationService.GetUserById(id);
        }

        [HttpPatch]
        public async Task<IActionResult> ChangePassword(UserDto userDto, string newPassword)
        {
            return await _userApplicationService.ChangePassword(userDto, newPassword);
        }
    }
}
