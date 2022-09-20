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

            var result = mapper.Map<ResultRequest, Result>(resultRequest);
            resultRepo.Add(result);

            UserResult winnerResult = new UserResult();
            winnerResult.ResultId = result.Id;
            winnerResult.UserId = winner.Id;

            UserResult loserResult = new UserResult();
            loserResult.ResultId = result.Id;
            loserResult.UserId = loser.Id;

            userResultRepo.Add(winnerResult);
            userResultRepo.Add(loserResult);

            var affectRowNumber = unitOfWork.Commit();


            if (affectRowNumber > 0)
                return new APISuccessResult<string>("Add result successfully!");
            return new APIErrorResult<string>("Add result fail!");
        }

        public APIResult<IEnumerable<ResultResponse>> GetResults(Expression<Func<Result, bool>> filter = null, Func<IQueryable<Result>, IOrderedQueryable<Result>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            var resultList = resultRepo.GetList(filter, orderBy, includeProperties, skip, take);
            if (resultList == null)
                return new APIErrorResult<IEnumerable<ResultResponse>>("Get result list failed!");
            var response = mapper.Map<IEnumerable<ResultResponse>>(resultList);
            return new APISuccessResult<IEnumerable<ResultResponse>>(response);
        }
    }
}
