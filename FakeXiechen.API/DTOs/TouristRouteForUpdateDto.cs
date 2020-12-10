using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class TouristRouteForUpdateDto : TouristRouteForBaseDto
    {
        // 如果通过继承对不同的DTO字段做出验证规则，那就是重写新的验证规则字段

        [Required(ErrorMessage = "更新必备")]
        [MaxLength(1500)]
        public override string Description { get; set; }

    }
}
