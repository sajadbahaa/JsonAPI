using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Model
{
    public class LoginUserDto
    {
    public string Email { get; set; }
    public string Password { get; set; }
    }

    public class ResponseUserDto
    {
        public string Token { get; init; } = null!;
    }
}
