using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageEventsReviewUoW
    {
        Task<EventReview> AddEventReview(EventReviewDto eventReviewDto);
        Task<bool> CheckIfReviewExistForThisEventAndUser(EventReviewDto eventReviewDto);
        Task<double> GetAverageRatingForEvent(int eventId);
    }
}
