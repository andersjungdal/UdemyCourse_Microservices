﻿using Actio.Common.Exceptions;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly ICategoryRepository categoryRepository;

        public ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository)
        {
            this.activityRepository = activityRepository;
            this.categoryRepository = categoryRepository;
        }
        public async Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdAt)
        {
            var activityCategory = await categoryRepository.GetAsync(name);
            if(activityCategory == null)
            {
                throw new ActioExceptions("category_not_found", $"Category: {category} was not found.");
            }
            var activity = new Activity(id, activityCategory, userId, name, description, createdAt);
            await activityRepository.AddAsync(activity);
        }
    }
}
