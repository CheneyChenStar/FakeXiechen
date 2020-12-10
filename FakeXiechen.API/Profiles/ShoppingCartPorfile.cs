using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;

namespace FakeXiechen.API.Profiles
{
    public class ShoppingCartPorfile : Profile
    {
        public ShoppingCartPorfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();

        }
    }
}
