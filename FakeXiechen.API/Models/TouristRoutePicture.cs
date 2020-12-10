using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Models
{
    public class TouristRoutePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //设置了主键唯一且自增添加,因此添加数据库时无需指定ID
        public int Id { get; set; }
        [MaxLength(100)]
        public string Url { get; set; }
        [ForeignKey("TouristRouteId")]
        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }

    }
}
