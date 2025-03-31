using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models.Dto
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OwnerId { get; set; }
    
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Name) || StartDate == null || EndDate == null || OwnerId == 0)
            {
                return false;
            }
            return true;
        }
    }
}
