using Actio.Common.Auth;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;
using Actio.Services.Identity.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Actio.Services.Identity.Tests.Unit.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task user_service_login_should_return_jwt()
        {
            string email = "test@test.com";
            string password = "secret";
            string name = "test";
            string salt = "salt";
            string hash = "hash";
            string token = "token";
            var userRepositoryMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            encrypterMock.Setup(x => x.GetSalt(password)).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);
            jwtHandlerMock.Setup(a => a.Create(It.IsAny<Guid>())).Returns(new JsonWebToken
            {
                Token = token
            });
            User user = new User(email, name);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(s => s.GetAsync(email)).ReturnsAsync(user);

            var userService = new UserService(userRepositoryMock.Object, encrypterMock.Object, jwtHandlerMock.Object);

            var jwt = await userService.LoginAsync(email, password);
            userRepositoryMock.Verify(x => x.GetAsync(email), Times.Once);
            jwtHandlerMock.Verify(x => x.Create(It.IsAny<Guid>()), Times.Once);
            jwt.Should().NotBeNull(); 
        }
    }
}
