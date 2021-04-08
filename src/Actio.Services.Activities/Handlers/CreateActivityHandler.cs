using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exceptions;
using Actio.Services.Activities.Services;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService activityService;
        private Microsoft.Extensions.Logging.ILogger _logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService, ILogger<CreateActivityHandler> logger)
        {
            _busClient = busClient;
            _logger = logger;
            this.activityService = activityService;
        }
        public async Task HandleAsync(CreateActivity command)
        {
            _logger.LogInformation($"Creating Activity: {command.Name}");
            try
            {
                await activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt);
                await _busClient.PublishAsync(new ActivityCreated(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt));
                return;
            }
            catch (ActioExceptions ex)
            {

                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, ex.Code, ex.Message));
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {

                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, "error", ex.Message));
                _logger.LogError(ex.Message);

            }

        }
    }
}
