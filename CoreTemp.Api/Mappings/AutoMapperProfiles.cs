using AutoMapper;
using CoreTemp.Common.Common;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DTOs.Order;
using CoreTemp.Data.DTOs.Product;
using CoreTemp.Data.DTOs.Slider;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Data.Models.Site;
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
            CreateMap<MyUser, UserForDetailedDto>();
            CreateMap<MyUser, UserProfileDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.ToAge());
                });


            CreateMap<MyUser, UserForLoginDto>();

            CreateMap<ProductGroupDto,ProductGroup>();
            CreateMap<ProductGroup,ProductGroupDto>();

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<SliderDto, Slider>();
            CreateMap<Slider, SliderDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDto>();

            CreateMap<PaymentLogDto, PaymentLog>();
            CreateMap<PaymentLog, PaymentLogDto>();
        }

    }
}
