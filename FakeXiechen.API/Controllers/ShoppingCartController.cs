using FakeXiechen.API.Servers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;

namespace FakeXiechen.API.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartController( 
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _touristRouteRepository = touristRouteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // 获取购物车
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetShoppingCartAsync()
        {
            // 1.获取当前用户id.此处需要获取Http上下文
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 2.根据用户id获取对应的购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);
            // 3.返回购物车dto
            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(shoppingCart);

            return Ok(shoppingCartDto);
        }
 
        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem(
            [FromBody] AddShoppingCarItemDto  itemDto
            )
        {
            if (itemDto == null)
            {
                return BadRequest();
            }
            // 1.获取购物车
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);
            // 2.将LineItemDto映射为LineItem，并添加进购物车
            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(itemDto.TouristRouteId);
            if (touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }
            var lineItem = new LineItem()
            { 
                TouristRouteId = itemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent  = touristRoute.DiscountPresent
            };

            //shoppingCart.ShoppingCartItems.Add(lineItem);
            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();
            // 3.返回NoContent
            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(shoppingCart);

            return Ok(shoppingCartDto);
        }

    }
}
