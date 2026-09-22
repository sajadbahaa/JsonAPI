using Application.Interface;
using Application.Model;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JsonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
    
    public AuthController(IAuth authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromQuery] LoginUserDto loginDto)
        {
         var response = _authService.LoginUser(loginDto);
         return Ok(response);  
        }
    }
}
