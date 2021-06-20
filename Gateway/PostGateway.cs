using Chatter.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chatter.Gateway
{
    public class PostGateway:IPostGateway

    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PostGateway(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<Models.Post> GetPost(int postid, Models.Post post)
        {
            var response = await _httpClient.GetAsync(_configuration["ChatterAPI"] + "/" + postid);
            string apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Models.Post>(apiResponse);
        }

        public async Task<List<Models.Post>> GetPosts()
        {
            var response = await _httpClient.GetAsync(_configuration["ChatterAPI"]);
            string apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Models.Post>>(apiResponse);
        }

        public async Task<Models.Post> PostPost(Models.Post Post)
        {
            var response = await _httpClient.PostAsJsonAsync(_configuration["ChatterAPI"], Post);
            Models.Post returnValue = await response.Content.ReadFromJsonAsync<Models.Post>();

            return returnValue;
        }
        public async Task<Models.Post> DeletePost(int deleteId)
        {
            var response = await _httpClient.DeleteAsync(_configuration["ChatterAPI"] + "/" + deleteId);
            Post returnValue = await response.Content.ReadFromJsonAsync<Post>();
            return returnValue;
        }

        public async Task PutPost(int editId, Models.Post Post)
        {
            var response = await _httpClient.PutAsJsonAsync(_configuration["ChatterAPI"] + "/" + editId, Post);
           
        }



    }
}
