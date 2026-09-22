using Application.Interface;
using Application.Model;
using Domain.User;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuth
    {
        private ITokenService _tokenService;
        private readonly UserService _userService;
        private readonly ILogger<AuthService> _logger;  
        public AuthService(UserService userService,ITokenService tokenService,ILogger<AuthService> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }
       
       
       public ResponseUserDto LoginUser(LoginUserDto loginDto)
        {
            var user = _userService.GetUserByEmail(loginDto.Email);

            if (!_userService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid Credintials");
            }
            _logger.LogInformation("Generated Token Successfully");
            return _tokenService.GenerateToken(user);
        }
    }
}
