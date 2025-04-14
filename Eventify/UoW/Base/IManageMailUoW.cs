using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.UoW.Base
{
    public interface IManageMailUoW
    {
        Task<bool> SendEventParticipantEmail(int userId, int eventId);
    }
}
