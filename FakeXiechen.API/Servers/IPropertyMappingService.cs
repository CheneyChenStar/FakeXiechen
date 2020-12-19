using System.Collections.Generic;

namespace FakeXiechen.API.Servers
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsMappingExits<TSource, TDestination>(string fields);

        bool IsPropertiesExists<T>(string fields);
    }
}