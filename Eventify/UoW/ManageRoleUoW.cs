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
    public class ManageRoleUoW : IManageRolesUoW
    {
        private EventifyDbContext _context;
        private IMapper _mapper;

        public ManageRoleUoW(EventifyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Role?> GetRoleName(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync( role => role.Id == roleId);

            return role;
        }
    }
}
