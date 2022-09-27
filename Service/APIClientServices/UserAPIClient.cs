using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Service.APIClientServices
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
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
                return new ApiErrorResult<string>($"Cannot connect to server because {ex.Message}");
            }
        }
        public async Task<ApiResult<bool>> Delete(DeleteUserRequest deleteUserRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                if (token == null)
                    return new ApiErrorResult<bool>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var user = await GetByUserName(deleteUserRequest.UserName);
                if (!user.Succeeded)
                    return new ApiErrorResult<bool>("User Not Found!");
                var response = await client.DeleteAsync($"User/Delete/{deleteUserRequest.UserName}");
                return await ResultReturn<bool>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Cannot connect to server because {ex.Message}");
            }

        }

        public async Task<ApiResult<UserResponse>> GetByUserName(string userName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                if (token == null)
                    return new ApiErrorResult<UserResponse>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"User/GetByUserName/{userName}");
                return await ResultReturn<UserResponse>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<UserResponse>($"Cannot connect to server because {ex.Message}");
            }

        }

        public async Task<ApiResult<PagedList<UserResponse>>> GetPagedList(PagingRequest pagingRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                // What if token expires
                if (token == null)
                    return new ApiErrorResult<PagedList<UserResponse>>("Unauthorized");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"User/GetPagedList?{nameof(pagingRequest.PageIndex)}={pagingRequest.PageIndex}&{nameof(pagingRequest.PageSize)}={pagingRequest.PageSize}");
                return await ResultReturn<PagedList<UserResponse>>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PagedList<UserResponse>>($"Cannot connect to server because {ex.Message}");
            }

        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
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
                return new ApiErrorResult<bool>($"Cannot connect to server because {ex.Message}");
            }
        }

        public async Task<ApiResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResult>>(await response.Content.ReadAsStringAsync())!;
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<TResult>>(await response.Content.ReadAsStringAsync())!;
        }

        public async Task<ApiResult<bool>> RoleAssign(string userName, RoleAssignRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"User/RoleAssign/{userName}", httpContent);
                return await ResultReturn<bool>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Cannot connect to server because {ex.Message}");
            }

        }

        public async Task<ApiResult<bool>> Update(string userName, UpdateUserRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CaroAPI");
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"User/Update/{userName}", httpContent);
                return await ResultReturn<bool>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Cannot connect to server because {ex.Message}");
            }

        }
    }
}
