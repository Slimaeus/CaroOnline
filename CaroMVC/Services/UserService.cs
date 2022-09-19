using Model.RequestModels;
using Newtonsoft.Json;

namespace CaroMVC.Services
{
    public class UserService
    {
        public UserService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string APIUrl { 
            get
            {
                return Configuration.GetValue<string>("CaroAPIBaseUrl");
            }
        }
        public HttpResponseMessage Login(LoginRequest loginModel)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(APIUrl);
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginModel));
            HttpResponseMessage  response = client.PostAsync("User/Login", content).GetAwaiter().GetResult();
            //if (response.IsSuccessStatusCode)
            //    return 
            return response;
        }
    }
}
