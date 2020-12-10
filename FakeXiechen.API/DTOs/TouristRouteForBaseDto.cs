using FakeXiechen.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    [TRTitleMustBeDifferentFromDescription]
    public abstract class TouristRouteForBaseDto
    {
        [Required(ErrorMessage = "Title 不可为空或超过了100字符")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description 不可为空或超过了1500字符")]
        [MaxLength(1500)]
        public virtual string Description { get; set; }

        public decimal Price { get; set; }

        //public decimal OriginalPrice { get; set; } //originalPrice
        //public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureForCreationDto>();
    }
}
