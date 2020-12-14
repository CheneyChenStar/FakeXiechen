using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }  //外键
        public ICollection<LineItem> ShoppingCartItems { get; set; }
            = new List<LineItem>();
        public string State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TranscationMetadata { get; set; }

    }
}
