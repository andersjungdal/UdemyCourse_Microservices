using Actio.Api.Controllers;
using Actio.Api.Repositories;
using Moq;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Actio.Common.Commands;
using FluentAssertions;

namespace Actio.Api.Tests.Controllers
{
    public class ActivitiesControllerTests
    {
        [Fact]
        public async void activities_controller_post_should_return_accepted()
        {
            var busClientMock = new Mock<IBusClient>();
            var activityRepositoryMock = new Mock<IActivityRepository>();
            var controller = new ActivitiesController(busClientMock.Object, activityRepositoryMock.Object);
            var userId = Guid.NewGuid();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity
                    (
                        new Claim[] {new Claim(ClaimTypes.Name, userId.ToString())}
                    ))
                }
            };
            var command = new CreateActivity()
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };
            var result = await controller.Post(command);

            var contentResult = result as AcceptedResult;
            contentResult.Should().NotBeNull();
        }
    }
}
