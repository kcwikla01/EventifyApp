using Eventify.Database.DbContext;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Eventify.WEB.ApplicationServices
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly EventifyDbContext _dbContext;
        public UserApplicationService(EventifyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IActionResult CreateUser(UserDto userDto)
        {
            if (!IsValidEmail(userDto.Email))
            {
                return new BadRequestObjectResult("Invalid email format.");
            }

            User user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                RoleId = 2
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return new OkObjectResult(userDto);
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
}
