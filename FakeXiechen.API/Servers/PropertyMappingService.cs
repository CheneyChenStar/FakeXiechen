using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FakeXiechen.API.Servers
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _touristRoutePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"Title", new PropertyMappingValue(new List<string>(){"Title"}) },
                {"Rating", new PropertyMappingValue(new List<string>(){"Rating"}) },
                {"OriginalPrice", new PropertyMappingValue(new List<string>(){"OriginalPrice"}) },
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<TouristRouteDto, TouristRoute>(_touristRoutePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue>
            GetPropertyMapping<TSource, TDestination>()
        {
            var matchMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchMapping.Count() == 1)
            {
                return matchMapping.First()._mappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance for<{typeof(TSource)},{typeof(TDestination)}>");
        }

        public bool IsMappingExits<TSource, TDestination>(string fields )
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(",");

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedField
                    : trimmedField.Remove(indexOfFirstSpace);
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsPropertiesExists<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(",");

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    //没有对应属性名
                    return false;
                }
            }
            return true;
        }
    }
}
