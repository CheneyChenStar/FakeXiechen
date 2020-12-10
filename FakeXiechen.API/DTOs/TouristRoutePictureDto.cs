using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.DTOs
{
    public class TouristRoutePictureDto
    {

        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TouristRouteId { get; set; }
    }
}
