using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }  //外键
        public ICollection<LineItemDto> ShoppingCartItem { get; set; } //商品  
            = new List<LineItemDto>();
    }
}
