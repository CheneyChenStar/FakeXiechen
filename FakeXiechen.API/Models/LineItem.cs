using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Models
{
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   //指定整数主键自增
        public int Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }

        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }

        [ForeignKey("TouristRouteId")]
        public Guid TouristRouteId { get; set; }    //外键

        public Guid? ShoppingCartId { get; set; }   //外键

        //public Guid? OrderId { get; set; }

        public TouristRoute TouristRoute { get; set; }

    }
}
