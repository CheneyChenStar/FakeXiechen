using AutoMapper;
using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => src.State.ToString())
                );
        }
    }
}
