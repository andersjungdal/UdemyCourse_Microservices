using Actio.Api.Repositories;
using Actio.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IActivityRepository activityRepository;

        public ActivityCreatedHandler(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }
        public async Task HandleAsync(ActivityCreated @event)
        {
            await activityRepository.AddAsync(new Models.Activity
            { 
                Id = @event.Id,
                UserId = @event.UserId,
                Name = @event.Name,
                Description = @event.Description,
                Category = @event.Category,
                CreatedAt = @event.CreatedAt
            });
            Console.WriteLine($"Activity created: {@event.Name}");
        }
    }
}
