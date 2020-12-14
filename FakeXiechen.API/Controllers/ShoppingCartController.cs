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
using FakeXiecheng.API.Helper;

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
        public async Task<IActionResult> AddShoppingCartItemAsync(
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
            var lineItem = new LineItem()   // 这个商品并未直接添加进shoppingCart的商品List列表中而是用外键表明关系，
            {                               // 因此访问数据库获取shoppingCart时，可以联合几张相关的表查询返回结果，反序列化ShoppingCart对象
                TouristRouteId = itemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent  = touristRoute.DiscountPresent,
                TouristRoute = touristRoute
            };

            //shoppingCart.ShoppingCartItems.Add(lineItem);
            await _touristRouteRepository.AddShoppingCartItemAsync(lineItem);
            await _touristRouteRepository.SaveAsync();
            // 3.返回NoContent

            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(shoppingCart);

            return Ok(shoppingCartDto);
        }

        [HttpDelete("items/{itemsId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteLineItemFromShoppingCartAsync(
            [FromRoute] int itemsId
            )
        {
            //1.获取对应的LineItem
            LineItem lineItem = await _touristRouteRepository.GetLineItemByItemIdAsync(itemsId);
            if (lineItem == null)
            {
                return NotFound("购物车商品未找到");
            }
            // 2.数据库中移除
            _touristRouteRepository.DeleteLineItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            // 3.返回
            return NoContent();
        }

        [HttpDelete("items/({itemIds})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteLineItemsByIdsAsync(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            [FromRoute] IEnumerable<int> itemIds
            )
        {
            // 1.获取LineItem
            IEnumerable<LineItem> lineItems = await _touristRouteRepository.GetLineItemsByItemIdsAsync(itemIds);
            if (lineItems == null)
            {
                return NotFound("列表商品未找到");
            }
            // 2.数据库中批量移除
            _touristRouteRepository.DeleteLineItems(lineItems);
            await _touristRouteRepository.SaveAsync();
            // 3.返回
            return NoContent();
        }
        
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutAsync()
        {
            //1.获取当前用户id
            var userId = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;
            //2.获取购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);
            //3.创建订单
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                CreateDateUTC = DateTime.UtcNow,
                ShoppingCartItems = shoppingCart.ShoppingCartItems,
                State = OrderStateEnum.Pending,
                UserId = userId,
            };
            shoppingCart.ShoppingCartItems = null;
            //4.保存订单
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();
            //5.返回dto对象
            return Ok(_mapper.Map<OrderDto>(order));
        }

    }
}
