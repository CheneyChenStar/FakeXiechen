using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
        public Guid TouristRouteId { get; set; }    
        public Guid? ShoppingCartId { get; set; }  
        public TouristRouteDto TouristRouteDto { get; set; }
    }
}
