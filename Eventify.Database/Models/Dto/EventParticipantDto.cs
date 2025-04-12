using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models.Dto
{
    public class EventParticipantDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
