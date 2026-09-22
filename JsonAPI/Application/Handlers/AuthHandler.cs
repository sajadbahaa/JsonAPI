using Application.options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Handlers
{
    
    public class AuthHandler:DelegatingHandler
    {
        private ILogger<AuthHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthHandler(IHttpContextAccessor httpContextAccessor, ILogger<AuthHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            
            if (request.Options.TryGetValue(
                RequestOptions.RequireAuth,
                out bool requiresAuth)
            && requiresAuth)
            {

               var token =   _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                _logger.LogInformation($"Authorization header: {token} \n  and Lenght: {token?.Length}");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
