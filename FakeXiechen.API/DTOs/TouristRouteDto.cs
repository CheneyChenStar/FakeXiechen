using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class TouristRouteDto
    {

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        // Price = OrininalPrice * DiscountPresent
        public decimal Price { get; set; }
        //public decimal OrininalPrice { get; set; }

        //public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        
        public string Feature { get; set; }

        public string Fees { get; set; }
        public string Notes { get; set; }
        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }

        public ICollection<TouristRoutePictureDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureDto>();

    }
}
