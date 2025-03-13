using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Database.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<EventReview> EventReviews { get; set; }
        public ICollection<EventSchedule> EventSchedules { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
