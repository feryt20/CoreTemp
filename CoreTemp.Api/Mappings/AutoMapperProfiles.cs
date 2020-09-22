using AutoMapper;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedDto>();
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.ToAge());
                });

            CreateMap<UserForUpdateDto, User>();

            CreateMap<User, UserForLoginDto>();
        }

    }
}
