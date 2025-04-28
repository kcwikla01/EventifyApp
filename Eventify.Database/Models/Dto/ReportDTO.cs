using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models.Dto
{
    public class ReportDTO
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public int CountOfParticipants { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
