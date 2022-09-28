﻿using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.APIClientServices;

public interface IResultApiClient
{
    Task<ApiResult<bool>> Create(ResultRequest request);
    Task<ApiResult<PagedList<ResultResponse>>> GetPagedList(PagingRequest request);
}