using AutoMapper;
using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
///  自动映射模型至DTO的关系
/// </summary>
namespace FakeXiechen.API.Profiles
{
    public class TouristRouteProfile : Profile
    {
        public TouristRouteProfile()
        {
            // TouristRoute => TouristRouteDto 映射关系
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember( // 定义成员映射规则
                    dest => dest.Price, // 指定映射成员
                    opt => opt.MapFrom(src=>src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1)) //指定映射关系
                )
                .ForMember(
                    dest => dest.TravelDays,
                    opt=> opt.MapFrom(src => src.TravelDays.ToString())
                )
                .ForMember(
                    dest => dest.TripType,
                    opt => opt.MapFrom(src => src.TripType.ToString())
                )
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.MapFrom(src => src.DepartureCity.ToString())
                )
                ;
            // TouristRouteForCreationDto => TouristRoute 映射关系
            CreateMap<TouristRouteForCreationDto, TouristRoute>()
                .ForMember(
                    dest => dest.Id, // 指定映射成员
                    opt =>opt.MapFrom(src=> Guid.NewGuid()) //指定映射关系:返回一个Guid给指定映射成员
                )
                ;


            CreateMap<TouristRouteForUpdateDto, TouristRoute>();
            CreateMap<TouristRoute, TouristRouteForUpdateDto>();
        }
    }
}
