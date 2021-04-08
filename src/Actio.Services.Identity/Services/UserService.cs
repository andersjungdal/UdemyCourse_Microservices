using Actio.Common.Auth;
using Actio.Common.Exceptions;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEncrypter encrypter;
        private readonly IJwtHandler jwtHandler;

        public UserService(IUserRepository userRepository, IEncrypter encrypter, IJwtHandler jwtHandler)
        {
            this.userRepository = userRepository;
            this.encrypter = encrypter;
            this.jwtHandler = jwtHandler;
        }
        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await userRepository.GetAsync(email);
            if(user != null)
            {
                throw new ActioExceptions("email_in_use", "Email is already in use.");
            }
            user = new User(email, name);
            user.SetPassword(password, encrypter);
            await userRepository.AddAsync(user);
        }
        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioExceptions("invalid_credentials", "Invalid_credentials.");
            }
            if (!user.ValidatePassword(password, encrypter))
            {
                throw new ActioExceptions("invalid_credentials", "Invalid_credentials.");
            }

            return jwtHandler.Create(user.Id);
        }

    }
}
