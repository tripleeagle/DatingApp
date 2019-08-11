using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Api.Auth.Extensions
{
    public static class EnumExtension
    {
        public static string Description<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum)) return null; // could also return string.Empty
            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture)) continue;
                var memInfo = type.GetMember(type.GetEnumName(val));

                if (memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }

            return null; // could also return string.Empty
        }

        public static T ToEnum<T>(this string val)
        {
            return (T) Enum.Parse(typeof(T), val);
        }
        
        public static T GetEnum<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T) field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T) field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

    }
}