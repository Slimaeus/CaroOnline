using AutoMapper;
using Data;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.APIServices
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository resultRepo;
        private readonly IUserResultRepository userResultRepo;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly CaroDbContext dbContext;

        public ResultService(
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IResultRepository resultRepo, 
            IUserResultRepository userResultRepo, 
            UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.dbContext = unitOfWork.DbContext;
            this.resultRepo = new ResultRepository(dbContext);
            this.userResultRepo = new UserResultRepository(dbContext);
        }
        public async Task<APIResult<string>> AddResult(ResultRequest resultRequest)
        {
            var winner = await userManager.FindByNameAsync(resultRequest.WinnerUserName);
            var loser = await userManager.FindByNameAsync(resultRequest.LoserUserName);
            if (winner == null)
            {
                return new APIErrorResult<string>("Cannot found Winner!");
            }
            if (loser == null)
            {
                return new APIErrorResult<string>("Cannot found Loser!");
            }
            var result = mapper.Map<ResultRequest, Result>(resultRequest);
            resultRepo.Add(result);

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

            userResultRepo.Add(winnerResult);
            userResultRepo.Add(loserResult);

            var affectRowNumber = unitOfWork.Commit();


            if (affectRowNumber > 0)
                return new APISuccessResult<string>("Add result successfully!");
            return new APIErrorResult<string>("Add result fail!");
        }

        public APIResult<IEnumerable<ResultResponse>> GetResults(PagingRequest pagingRequest)
        {
            var resultList = resultRepo.GetList(
                skip: (pagingRequest.PageIndex - 1) * pagingRequest.PageSize,
                take: pagingRequest.PageSize
            );
            if (resultList == null)
                return new APIErrorResult<IEnumerable<ResultResponse>>("Get result list failed!");
            var response = mapper.Map<IEnumerable<ResultResponse>>(resultList);
            return new APISuccessResult<IEnumerable<ResultResponse>>(response);
        }

        public async Task<APIResult<IEnumerable<ResultResponse>>> GetResultsByUserName(string userName, PagingRequest pagingRequest)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new APIErrorResult<IEnumerable<ResultResponse>>("User not found!");
            }
            var resultList = resultRepo.GetList(
                    includeProperties: "UserResults",
                    filter: result => result.UserResults.Any(userResult => userResult.UserId == user.Id),
                    skip: (pagingRequest.PageIndex - 1) * pagingRequest.PageSize,
                    take: pagingRequest.PageSize
                );
            if (resultList == null)
                return new APIErrorResult<IEnumerable<ResultResponse>>("Get Result by UserName list failed!");
            var response = mapper.Map<IEnumerable<ResultResponse>>(resultList);
            return new APISuccessResult<IEnumerable<ResultResponse>>(response);
        }
    }
}
