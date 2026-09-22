using Application.options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientService> _logger;
        public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
        {
         _httpClient = httpClient;
         _logger = logger;
        }

        private HttpRequestMessage BuildRequest(HttpMethod method,Uri path,bool requireAuth, Dictionary<string, object>? parameters = null)
        {

            var uriBuilder = new UriBuilder(path);

            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            if (parameters is not null)
            {
                foreach (var (key, value) in parameters)
                {
                    query[$"param_{key}"] = value?.ToString() ?? @"\N";
                }
            }

            uriBuilder.Query = query.ToString();

            var request  = new HttpRequestMessage(method,uriBuilder.Uri);

            request.Options.Set(RequestOptions.RequireAuth, requireAuth);
            return request;
        }
        

        public async Task<T?> SendAsync<T>(Uri path,HttpMethod method,bool requireAuth,CancellationToken cancellationToken, Dictionary<string, object>? parameters = null)
        {
            var jsonOptions = new JsonSerializerOptions
            {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var request = BuildRequest(method, path, requireAuth,parameters);

            _logger.LogDebug("Sending HTTP {Method} to {Uri}", method.Method, path);

           var response = await _httpClient.SendAsync(request, cancellationToken);
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException("Not Found Data");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);

                    throw new JsonException(
                     $"External request failed: {errorBody}");
                }

                if (response.Content.Headers.ContentLength==0)
                {
                    return default;
                }

                var mapping = await response.Content.ReadFromJsonAsync<T?>(jsonOptions, cancellationToken);
                _logger.LogInformation("Deserialized response successfully");
                
                return mapping;
            }
            catch (JsonException ex)
            {
            throw new HttpRequestException("External Service", ex, System.Net.HttpStatusCode.BadGateway);
            }

         }
        
    }
    
}
