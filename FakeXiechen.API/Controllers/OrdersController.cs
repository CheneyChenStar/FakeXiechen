using FakeXiechen.API.Models;
using FakeXiechen.API.Servers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiechen.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using FakeXiechen.API.ResourceParameters;

namespace FakeXiechen.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _touristRouteRepository = touristRouteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] PaginationResourceParameters paginationResource)
        {
            // 1.获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 2.通过userId获取该用户的所有订单
            var orders = await _touristRouteRepository.GetOrdersByUserIdAsync(
                            userId,paginationResource.PageNumber, paginationResource.PageSize);
            // 3.返回所有订单的dto
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] Guid orderId)
        {
            // 1.获取用户id
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 2.根据用户id和orderid获取单一订单
            var order = await _touristRouteRepository.GetOrderByUserIdAndOrderIdAsync(userId, orderId);
            // 3.返回结果
            return Ok(_mapper.Map<OrderDto>(order));
        }


    }
}
