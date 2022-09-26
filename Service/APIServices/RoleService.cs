using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;

namespace Service.APIServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<ApiResult<bool>> Create(RoleRequest roleResponse)
        {
            var role = await _roleManager.FindByNameAsync(roleResponse.Name);
            if (role != null)
            {
                return new ApiErrorResult<bool>("Role existed!");
            }
            var newRole = _mapper.Map<AppRole>(roleResponse);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>("Role created failure!");
        }

        public async Task<ApiResult<IEnumerable<RoleResponse>>> GetList()
        {
            var roleList = await _roleManager.Roles.ToListAsync();
            if (roleList == null)
            {
                return new ApiErrorResult<IEnumerable<RoleResponse>>("Get Result list failure");
            }
            var roleResponseList = _mapper.Map<IEnumerable<RoleResponse>>(roleList);
            return new ApiSuccessResult<IEnumerable<RoleResponse>>(roleResponseList);
        }
    }
}
