using Data;
using Data.Repositories;
using Data.UnitOfWork;
using Model.DbModels;
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

        //private readonly IUnitOfWork unitOfWork;
        //private readonly IResultRepository resultRepository;

        //public ResultService(IUnitOfWork unitOfWork, IResultRepository resultRepository)
        //{
        //    this.unitOfWork = unitOfWork;
        //    this.resultRepository = resultRepository;
        //}

        //public bool AddResult(Result result)
        //{
        //    bool isSuccess = resultRepository.Add(result);
        //    return isSuccess;
        //}

        //public IEnumerable<Result> GetResults(Expression<Func<Result, bool>> filter = null!, Func<IQueryable<Result>, IOrderedQueryable<Result>> orderBy = null!, string includeProperties = "", int skip = 0, int take = 0)
        //{
        //    return resultRepository.GetList();
        //}
        public ResultService(IResultRepository resultRepo)
        {
            this.resultRepo = resultRepo;
        }
        public void AddResult(Result result)
        {
            resultRepo.Add(result);
        }

        public IEnumerable<Result> GetResults(Expression<Func<Result, bool>> filter = null, Func<IQueryable<Result>, IOrderedQueryable<Result>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return resultRepo.GetList(filter, orderBy, includeProperties, skip, take);
        }
    }
}
