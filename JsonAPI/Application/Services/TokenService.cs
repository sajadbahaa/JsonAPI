using Application.Interface;
using Application.Model;
using Domain.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenSettings
    {
    public int TimeOut { get; set; }
    }
    public class TokenService:ITokenService
    {
        private ILogger<TokenService> _logger;
        private readonly IOptionsMonitor<TokenSettings> _monitor;
        public TokenService(ILogger<TokenService> logger,IOptionsMonitor<TokenSettings> monitor)
        {
            _logger = logger;
            _monitor = monitor;
        }
        public ResponseUserDto GenerateToken(User user)
        {
            _logger.LogInformation($"Time Out For Expire Token : {_monitor.CurrentValue.TimeOut}");
            var token = new JwtSecurityToken(
                issuer: "JsonApi",
                audience: "JsonApiUsers",
                claims: GetCliamns(user),
                 expires: DateTime.Now.AddMinutes(_monitor.CurrentValue.TimeOut),
                 signingCredentials: signingCredentials()
             );
            return new ResponseUserDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        private SigningCredentials signingCredentials()
        {
            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
        private Claim[] GetCliamns(User user)
        {
            return new Claim[]{

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                new Claim(ClaimTypes.Email, user.Email),
                // Role (user  or Admin) used later for authorization
                new Claim(ClaimTypes.Role, user.Role)
        };
        }

        }
}
