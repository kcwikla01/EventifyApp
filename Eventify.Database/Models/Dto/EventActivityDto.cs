using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models.Dto
{
    public class EventActivityDto
    {
        public int eventId { get; set; }
        public string activityName { get; set; }
        public string activityDescription { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }


}
