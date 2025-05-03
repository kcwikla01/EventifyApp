using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventReviewApplicationService : IEventReviewApplicationService
    {
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageEventsReviewUoW _manageEventsReviewUoW;

        public EventReviewApplicationService(IManageEventsUoW manageEventsUoW, IManageEventsReviewUoW manageEventsReviewUoW)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageEventsReviewUoW = manageEventsReviewUoW;
        }
        public async Task<IActionResult> AddEventReview(EventReviewDto eventReviewDto)
        {
            if (eventReviewDto == null || eventReviewDto.EventId== null || eventReviewDto.Comment.Trim().Length==0 || eventReviewDto.UserId==0 || eventReviewDto.Rating==0)
            {
                return new BadRequestResult();
            }

            if (eventReviewDto.Rating > 0 && eventReviewDto.Rating < 5)
            {
                var eventById = await _manageEventsUoW.GetEventById(eventReviewDto.EventId);
                if(eventById == null)
                {
                    return new NotFoundObjectResult("Event not found.");
                }

                if (eventById.EndDate > DateTime.UtcNow)
                {
                    return new BadRequestObjectResult("Cannot add a review before the event has ended.");
                }

                var reviewExistForThisEventAndUser = await _manageEventsReviewUoW.CheckIfReviewExistForThisEventAndUser(eventReviewDto);
                if (!reviewExistForThisEventAndUser)
                {
                    var createdEventReview = await _manageEventsReviewUoW.AddEventReview(eventReviewDto);

                    if (createdEventReview != null)
                    {
                        return new OkObjectResult("Event review Added");
                    }
                    else
                    {
                        return new BadRequestObjectResult("Failed to create event review.");
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Review exist for this user");
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid rating value. Rating must be between 1 and 5.");
            }
        }
    }
}
