using FakeXiechen.API.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.ValidationAttributes
{
    public class TRTitleMustBeDifferentFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForBaseDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                        "路线名称与路线描述不可相同",
                        new[] { "TouristRouteForCreationDto" }
                    );
            }
            return ValidationResult.Success;
        }
    }
}
