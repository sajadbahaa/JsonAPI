using Application.Model;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface ITokenService
    {
    public ResponseUserDto GenerateToken(User user);
    }
}
