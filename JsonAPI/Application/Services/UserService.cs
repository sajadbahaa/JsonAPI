using Application.Interface;
using Application.Model;
using Domain.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UserService
    {
        // Login  : Search by Email And Password. 
        // check if password Correct then Gives Token in Response. 
        private ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public User GetUserByEmail(string Email)
        {
            var user = LoadedData.UsersList.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                throw  new KeyNotFoundException($"User with email '{Email}' not found.");
            }
            _logger.LogInformation("User with email  found.");
            return user;
        }

    public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
