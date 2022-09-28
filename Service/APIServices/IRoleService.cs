using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;

namespace Service.APIServices;

public interface IRoleService
{
    public Task<ApiResult<IEnumerable<RoleResponse>>> GetList();
    public Task<ApiResult<bool>> Create(RoleRequest roleResponse);
}