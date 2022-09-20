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
using System.Text;
using System.Threading.Tasks;

namespace Service.APIClientServices
{
    public class UserAPIClient : IUserAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<APIResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("CaroAPI");
            var response = await client.PostAsync("User/Authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<APISuccessResult<string>>(await response.Content.ReadAsStringAsync())!;
            }

            return JsonConvert.DeserializeObject<APIErrorResult<string>>(await response.Content.ReadAsStringAsync())!;
        }

        public Task<APIResult<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<APIResult<UserResponse>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<APIResult<IEnumerable<UserResponse>>> GetUserList(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "", int take = 0, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public Task<APIResult<bool>> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<APIResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<APIResult<bool>> Update(Guid id, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
