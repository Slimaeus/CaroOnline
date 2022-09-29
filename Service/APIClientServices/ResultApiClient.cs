using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Service.APIClientServices;
public class ResultApiClient : IResultApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ResultApiClient(IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<ApiResult<bool>> Create(ResultRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            // What if token expires
            if (token == null)
                return new ApiErrorResult<bool>("Unauthorized");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("result/create", httpContent);
            return await ResultReturn<bool>(response);
        }
        catch (Exception ex)
        {
            return new ApiErrorResult<bool>($"Cannot connect to server because {ex.Message}");
        }
    }
    public async Task<ApiResult<PagedList<ResultResponse>>> GetPagedList(PagingRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            if (token == null)
                return new ApiErrorResult<PagedList<ResultResponse>>("Unauthorized");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"result/get-list?{nameof(request.PageIndex)}={request.PageIndex}&{nameof(request.PageSize)}={request.PageSize}");
            return await ResultReturn<PagedList<ResultResponse>>(response);
        }
        catch (Exception ex)
        {
            return new ApiErrorResult<PagedList<ResultResponse>>($"Cannot connect to server because {ex.Message}");
        }
    }

    public async Task<ApiResult<IEnumerable<ResultResponse>>> GetResultByUserName(string userName, PagingRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CaroAPI");
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            if (token == null)
                return new ApiErrorResult<IEnumerable<ResultResponse>>("Unauthorized");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"result/get-by-username?username={userName}&{nameof(request.PageIndex)}={request.PageIndex}&{nameof(request.PageSize)}={request.PageSize}");
            return await ResultReturn<IEnumerable<ResultResponse>>(response);
        }
        catch (Exception ex)
        {
            return new ApiErrorResult<IEnumerable<ResultResponse>>($"Cannot connect to server because {ex.Message}");
        }
    }

    private async Task<ApiResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<ApiSuccessResult<TResult>>(await response.Content.ReadAsStringAsync())!;
        }
        return JsonConvert.DeserializeObject<ApiErrorResult<TResult>>(await response.Content.ReadAsStringAsync())!;
    }
}