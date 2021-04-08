using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exceptions;
using Actio.Services.Identity.Services;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IBusClient busClient;
        private readonly IUserService userService;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IBusClient busClient, IUserService userService, ILogger<CreateUserHandler> logger)
        {
            this.busClient = busClient;
            this.userService = userService;
            _logger = logger;
        }
        public async Task HandleAsync(CreateUser command)
        {
            _logger.LogInformation($"Creating user: {command.Email} {command.Name}");
            try
            {
                await userService.RegisterAsync(command.Email, command.Password, command.Name);
                await busClient.PublishAsync(new UserCreated(command.Email, command.Name));
                return;
            }
            catch (ActioExceptions ex)
            {

                await busClient.PublishAsync(new CreateUserRejected(command.Email, ex.Code, ex.Message));
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {

                await busClient.PublishAsync(new CreateUserRejected(command.Email, "error", ex.Message));
                _logger.LogError(ex.Message);

            }
        }
    }
}
