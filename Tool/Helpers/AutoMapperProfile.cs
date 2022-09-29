using AutoMapper;
using Model.ActionModels;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, UserResponse>();
        CreateMap<UserResponse, AppUser>();
        CreateMap<RoleRequest, AppRole>();
        CreateMap<AppRole, RoleResponse>();
        CreateMap<RegisterRequest, AppUser>();
        CreateMap<ResultRequest, Result>()
            .ForMember(des => des.StartedTime, act => act.MapFrom(src => DateTime.Now))
            .ForMember(
                des => des.EndedTime,
                act => act.MapFrom(
                    src => DateTime.Now
                        .AddHours(src.Hour)
                        .AddMinutes(src.Minute)
                        .AddSeconds(src.Second)
                ));
        CreateMap<Result, ResultResponse>();
        CreateMap<RegisterRequest, LoginRequest>();
        CreateMap<RegisterModel, LoginModel>()
            .AfterMap((src, des, context) =>
            {
                des.Input = context.Mapper.Map<LoginRequest>(src.Input);
            });
        CreateMap<ResultResponse, HistoryModel>();
        //CreateMap<PagedList<ResultResponse>, PagedList<HistoryModel>>()
        //    .AfterMap((src, des, context) =>
        //    {
        //        des.Items = context.Mapper.Map<IEnumerable<HistoryModel>>(src.Items);
        //    });
    }
}