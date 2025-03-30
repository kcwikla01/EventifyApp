using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageRoleUoW
    {
        Task<Role?> GetRoleName(int roleId);
    }
}
