using Model.RequestModels;
using Model.ResultModels;

namespace CaroMVC.Services
{
    public interface IUserAPIClient
    {
        public Task<APIResult<string>> Authorize(LoginRequest loginRequest);
        public Task<APIResult<string>> Register(RegisterRequest registerRequest);
        public Task<APIResult<string>> Update(UpdateUserRequest updateRequest);
    }
}
