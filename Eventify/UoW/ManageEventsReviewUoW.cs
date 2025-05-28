using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.DbContext;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Microsoft.EntityFrameworkCore;

namespace Eventify.UoW
{
    public class ManageEventsReviewUoW : IManageEventsReviewUoW
    {
        private readonly EventifyDbContext _context;

        public ManageEventsReviewUoW(EventifyDbContext context)
        {
            _context = context;
        }
        public async Task<EventReview> AddEventReview(EventReviewDto eventReviewDto)
        {
            EventReview eventReview = new EventReview
            {
                EventId = eventReviewDto.EventId,
                UserId = eventReviewDto.UserId,
                Comment = eventReviewDto.Comment,
                Rating = eventReviewDto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventReviews.Add(eventReview);
            await _context.SaveChangesAsync();

            return eventReview;
        }

        public async Task<bool> CheckIfReviewExistForThisEventAndUser(EventReviewDto eventReviewDto)
        {
            var review = await _context.EventReviews.FirstOrDefaultAsync(review =>
                review.EventId.Equals(eventReviewDto.EventId) && review.UserId.Equals(eventReviewDto.UserId));

            return review != null;
        }

        public async Task<double> GetAverageRatingForEvent(int eventId)
        {
            var averageRating = await _context.EventReviews
                .Where(review => review.EventId == eventId)
                .Select(r => (double?)r.Rating)
                .AverageAsync() ?? 0.0;

            return averageRating;
        }

        public async Task<List<string>> GetCommentsForEvent(int eventId)
        {
            var comments = await _context.EventReviews.Where(comment => comment.EventId == eventId)
                .Select(r => r.Comment)
                .ToListAsync();
            return comments;
        }
    }
}
