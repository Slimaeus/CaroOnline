using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service.APIClientServices
{
    public class UserAPIClient : IUserAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<APIResult<string>> Authenticate(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("User/Authenticate", httpContent);
            return await ResultReturn<string>(response);
        }

        public async Task<APIResult<bool>> Delete(DeleteUserRequest deleteUserRequest)
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var user = await GetByUserName(deleteUserRequest.UserName);
            if (!user.Succeeded)
                return new APIErrorResult<bool>("User Not Found!");
            var response = await client.DeleteAsync($"User/Delete?username={deleteUserRequest.UserName}");
            return await ResultReturn<bool>(response);    
        }

        public async Task<APIResult<UserResponse>> GetByUserName(string userName)
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"User/GetByUserName?userName={userName}");
            return await ResultReturn<UserResponse>(response);
        }

        public Task<APIResult<IEnumerable<UserResponse>>> GetUserList(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "", int take = 0, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResult<bool>> Register(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("User/Register", httpContent);
            return await ResultReturn<bool>(response);
        }

        public async Task<APIResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response)
        {
            string body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<APISuccessResult<TResult>>(body)!;
            }
            return JsonConvert.DeserializeObject<APIErrorResult<TResult>>(body)!;
        }

        public async Task<APIResult<bool>> RoleAssign(string userName, RoleAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"User/RoleAssign?username={userName}", httpContent);
            return await ResultReturn<bool>(response);
        }

        public Task<APIResult<bool>> Update(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
