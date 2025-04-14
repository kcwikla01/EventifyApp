using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models.Dto
{
    public class EventActivityDto
    {
        public int ActivityId { get; set; }
        public int EventId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }


}
