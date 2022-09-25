using AutoMapper;
using Data;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;

namespace Service.APIServices
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepo;
        private readonly IUserResultRepository _userResultRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ResultService(
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IResultRepository resultRepo, 
            IUserResultRepository userResultRepo, 
            UserManager<User> userManager)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._resultRepo = new ResultRepository(unitOfWork.DbContext);
            this._userResultRepo = new UserResultRepository(unitOfWork.DbContext);
        }
        public async Task<ApiResult<string>> AddResult(ResultRequest resultRequest)
        {
            var winner = await _userManager.FindByNameAsync(resultRequest.WinnerUserName);
            var loser = await _userManager.FindByNameAsync(resultRequest.LoserUserName);
            if (winner == null)
            {
                return new ApiErrorResult<string>("Cannot found Winner!");
            }
            if (loser == null)
            {
                return new ApiErrorResult<string>("Cannot found Loser!");
            }
            var result = _mapper.Map<ResultRequest, Result>(resultRequest);
            _resultRepo.Add(result);

            var winnerResult = new UserResult
            {
                ResultId = result.Id,
                UserId = winner.Id
            };

            var loserResult = new UserResult
            {
                ResultId = result.Id,
                UserId = loser.Id
            };

            _userResultRepo.Add(winnerResult);
            _userResultRepo.Add(loserResult);

            var affectRowNumber = _unitOfWork.Commit();


            if (affectRowNumber > 0)
                return new ApiSuccessResult<string>("Add result successfully!");
            return new ApiErrorResult<string>("Add result fail!");
        }

        public async Task<ApiResult<string>> DeleteResultById(Guid resultId, DeleteResultRequest resultRequest)
        {
            var result = await _resultRepo.GetByIdAsync(id: resultId);
            if (result == null)
                return new ApiErrorResult<string>("Game does not exist!");
            _resultRepo.Delete(result);
            var affectRowNumber = _unitOfWork.Commit();


            if (affectRowNumber > 0)
                return new ApiSuccessResult<string>("Delete result successfully!");
            return new ApiErrorResult<string>("Delete result fail!");
        }

        // Not working yet
        public async Task<ApiResult<string>> DeleteResultByUserName(string userName, DeleteResultRequest resultRequest)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiErrorResult<string>("Cannot found user!");
            }
            var resultIdList = _resultRepo.GetList(
                includeProperties: "UserResults",
                filter: result => result.UserResults.Any(userResult => userResult.UserId == user.Id)
            ).Select(result => result.Id);
            _resultRepo.Delete(res => resultIdList.Contains(res.Id));
            var affectRowNumber = _unitOfWork.Commit();


            if (affectRowNumber > 0)
                return new ApiSuccessResult<string>("Delete result successfully!");
            return new ApiErrorResult<string>("Delete result fail!");
        }

        public ApiResult<IEnumerable<ResultResponse>> GetResults(PagingRequest pagingRequest)
        {
            var resultList = _resultRepo.GetList(
                skip: (pagingRequest.PageIndex - 1) * pagingRequest.PageSize,
                take: pagingRequest.PageSize
            );
            if (resultList.Any())
                return new ApiErrorResult<IEnumerable<ResultResponse>>("Get result list failed!");
            var response = _mapper.Map<IEnumerable<ResultResponse>>(resultList);
            return new ApiSuccessResult<IEnumerable<ResultResponse>>(response);
        }

        public async Task<ApiResult<IEnumerable<ResultResponse>>> GetResultsByUserName(string userName, PagingRequest pagingRequest)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiErrorResult<IEnumerable<ResultResponse>>("User not found!");
            }
            var resultList = _resultRepo.GetList(
                    includeProperties: "UserResults",
                    filter: result => result.UserResults.Any(userResult => userResult.UserId == user.Id),
                    skip: (pagingRequest.PageIndex - 1) * pagingRequest.PageSize,
                    take: pagingRequest.PageSize
                );
            if (resultList == null)
                return new ApiErrorResult<IEnumerable<ResultResponse>>("Get Result by UserName list failed!");
            var response = _mapper.Map<IEnumerable<ResultResponse>>(resultList);
            return new ApiSuccessResult<IEnumerable<ResultResponse>>(response);
        }
    }
}
