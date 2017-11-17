using System;
using System.Linq;

namespace RoutableDto.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsOpenGenericType(this Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        public static bool IsGenericTypeOf(this Type type, Type genericType)
        {
            return type.GetInterfaces().Any(interfaceType =>
                interfaceType.IsGenericType
                && interfaceType.GetGenericTypeDefinition() == genericType);
        }

        public static T GetAttribute<T>(this Type tv) where T : Attribute
        {
            T[] attrs = (T[])tv.GetCustomAttributes(typeof(T), false);
            if (attrs.Length == 0)
                return null;
            if (attrs.Length == 1)
                return attrs[0];
            return attrs.FirstOrDefault(a => a.GetType() == typeof(T));

        }

        public static T[] GetAttributes<T>(this Type tv) where T : Attribute
        {
            return (T[])tv.GetCustomAttributes(typeof(T), false);
        }
    }
}
