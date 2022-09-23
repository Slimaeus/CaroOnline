using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;

        public UserAPIClient(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }
        public async Task<APIResult<string>> Authenticate(LoginRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("User/Authenticate", httpContent);
                return await ResultReturn<string>(response);
            }
            catch (Exception ex)
            {
                return new APIErrorResult<string>(ex.Message);
            }
        }

        public async Task<APIResult<bool>> Delete(DeleteUserRequest deleteUserRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _memoryCache.Get<String>("Token");
                if (token == null)
                    return new APIErrorResult<bool>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var user = await GetByUserName(deleteUserRequest.UserName);
                if (!user.Succeeded)
                    return new APIErrorResult<bool>("User Not Found!");
                var response = await client.DeleteAsync($"User/Delete?username={deleteUserRequest.UserName}");
                return await ResultReturn<bool>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<bool>(ex.Message);

            }
        }

        public async Task<APIResult<UserResponse>> GetByUserName(string userName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _memoryCache.Get<String>("Token");
                // What if token expires
                if (token == null)
                    return new APIErrorResult<UserResponse>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"User/GetByUserName?userName={userName}");
                return await ResultReturn<UserResponse>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<UserResponse>(ex.Message);

            }
        }

        public async Task<APIResult<PagedList<UserResponse>>> GetPagedList(PagingRequest pagingRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _memoryCache.Get<String>("Token");
                // What if token expires
                if (token == null)
                    return new APIErrorResult<PagedList<UserResponse>>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"User/GetPagedList?{nameof(pagingRequest.PageIndex)}={pagingRequest.PageIndex}&{nameof(pagingRequest.PageSize)}={pagingRequest.PageSize}");
                return await ResultReturn<PagedList<UserResponse>>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<PagedList<UserResponse>> (ex.Message);
            }
        }

        public Task<APIResult<IEnumerable<UserResponse>>> GetUserList(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "", int take = 0, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResult<bool>> Register(RegisterRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("User/Register", httpContent);
                return await ResultReturn<bool>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<bool>(ex.Message);
            }
        }

        public async Task<APIResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<APISuccessResult<TResult>>(body)!;
            }
            return JsonConvert.DeserializeObject<APIErrorResult<TResult>>(body)!;
        }

        public async Task<APIResult<bool>> RoleAssign(string userName, RoleAssignRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"User/RoleAssign?username={userName}", httpContent);
                return await ResultReturn<bool>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<bool>(ex.Message);
            }
        }

        public async Task<APIResult<bool>> Update(string userName, UpdateUserRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"User/Update?username={userName}", httpContent);
                return await ResultReturn<bool>(response);

            }
            catch (Exception ex)
            {
                return new APIErrorResult<bool>(ex.Message);
            }
        }
    }
}
