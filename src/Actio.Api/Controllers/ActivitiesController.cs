using Actio.Api.Repositories;
using Actio.Common.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Controllers
{
    [Route("api/activities")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IActivityRepository repository;

        public ActivitiesController(IBusClient busClient, IActivityRepository repository)
        {
            _busClient = busClient;
            this.repository = repository;
        }
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            command.UserId = Guid.Parse(User.Identity.Name);
            await _busClient.PublishAsync(command);
            return Accepted($"activities/{command.Id}");
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var activities = await repository.BrowseAsync(Guid.Parse(User.Identity.Name));
            return Json(activities.Select(x => new { x.Id, x.Name, x.Category, x.CreatedAt }));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            var activity = await repository.GetAsync(id);
            if(activity == null)
            {
                return NotFound();
            }
            if(activity.UserId != Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }


            return Json(activity);
        }

    }
}
