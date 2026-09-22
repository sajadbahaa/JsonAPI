using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JsonAPI.Authorization
{
    public class UserOwnerOrAdminHandler : AuthorizationHandler<UserOwnerOrAdminRequirementas,int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnerOrAdminRequirementas requirement,int userId)
        {
            // check Admin .

            if (context.User.IsInRole("Admin"))
            {
            context.Succeed(requirement);
            return Task.CompletedTask;
            }
            
            // check user 

                var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr,out int usId) && userId==usId)
                {
                 context.Succeed(requirement);
                 return Task.CompletedTask;
                }

            throw new ForbiddenException();
        }
    }
}
