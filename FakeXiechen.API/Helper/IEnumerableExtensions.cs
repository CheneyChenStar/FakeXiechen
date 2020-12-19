using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FakeXiechen.API.Helper
{
    public static class IEnumerableExtensions
    {
        //public static IEnumerable<ExpandoObject> ShapeData<TSource>
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> sources,
            string fields
            )
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }
            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfo = typeof(TSource)
                    .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfo);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"属性{propertyName}找不到" + $"{typeof(TSource)}");
                    }
                    propertyInfoList.Add(propertyInfo);

                }
            }

            foreach (TSource source in sources)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(source);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
