using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }  //外键
        public AppUser User { get; set; }   //反向导航
        public ICollection<LineItem> ShoppingCartItems { get; set; }
            = new List<LineItem>();
    }
}
