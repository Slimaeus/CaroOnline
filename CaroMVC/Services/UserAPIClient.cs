using Model.RequestModels;
using Model.ResultModels;
using Newtonsoft.Json;

namespace CaroMVC.Services
{
    public class UserAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserAPIClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<APIResult<string>> Authorize(LoginRequest loginRequest)
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var httpContent = new StringContent(json);
            var client = _httpClientFactory.CreateClient();
            var uri = _configuration.GetValue<string>("CaroAPIBaseUrl");
            client.BaseAddress = new Uri(uri);
            var response = await client.PostAsync("/User/Login", httpContent);
            if (response.Content == null)
                return new APIErrorResult<string>("Authorize Failed");
            var jsonContent = await response.Content.ReadAsStringAsync();
            if (jsonContent == null)
                return new APIErrorResult<string>("Authorize Failed");

            if (response.IsSuccessStatusCode)
            {
                var successResult = JsonConvert.DeserializeObject<APISuccessResult<string>>(jsonContent);
                if (successResult == null)
                    return new APIErrorResult<string>("Authorize Failed");
                return successResult;
            }
            var errorResult = JsonConvert.DeserializeObject<APIErrorResult<string>>(jsonContent);
            if (errorResult == null)
                return new APIErrorResult<string>("Authorize Failed");
            return errorResult;
        }

    }
}
