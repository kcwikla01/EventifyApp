using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eventify.Database.DbContext;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Microsoft.EntityFrameworkCore;

namespace Eventify.UoW
{
    public class ManageUsersUoW : IManageUsersUoW
    {
        private readonly EventifyDbContext _context;
        private readonly IMapper _mapper;

        public ManageUsersUoW(EventifyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public Task<UserDto> UpdateUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckIfUserExist(UserDto userDto)
        {
            return await _context.Users
                .AnyAsync(u => u.Name == userDto.Name || u.Email == userDto.Email);
        }

        public async Task<bool> ChangePassword(UserDto userDto, string newPassword)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Name == userDto.Name && u.Password == userDto.Password);
            if (user == null)
            {
                return false;
            }
            user.Password = newPassword;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> CheckIfUserPasswordCorrect(UserDto userDto)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Name == userDto.Name && u.Email == userDto.Email  && u.Password == userDto.Password);

            return user;
        }
    }
}
