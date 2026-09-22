using Application.Interface;
using JsonAPI.Model;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
namespace Application.Services
{
    public class PostService 
    {
        private  Uri _url;

        private HttpClientService _httpClientService;

        public PostService(HttpClientService httpClientService,IConfiguration configuration)
        {
         _httpClientService = httpClientService;
         _url =  new Uri (configuration["ExternalService:PostPath"] ?? throw new ArgumentNullException("ExternalService:PostPath is not configured."));
        }
       private Uri GetUrl(string path) => new Uri($"{_url}/{path}");
       public async Task<Post?> GetAsync(int id,CancellationToken cancellationToken)
       {
        var res = await _httpClientService.SendAsync<Post?>(GetUrl($"{id}"), HttpMethod.Get,false,cancellationToken: cancellationToken);
            if (res == null)
                throw new KeyNotFoundException("Not Found Post By Id");
            
            return res;
       }
    
    public async Task<List<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            var res = await _httpClientService.SendAsync<List<Post>>(_url, HttpMethod.Get, true, cancellationToken: cancellationToken);
            
            return res == null?[]:res;
        }

    }
}