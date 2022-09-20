using AutoMapper;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();
            CreateMap<RegisterRequest, User>();
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
        }
    }
}
