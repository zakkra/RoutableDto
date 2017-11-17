using System;
using System.Collections.Generic;
using System.Linq;

using RoutableDto.Extensions;

namespace RoutableDto.Interfaces
{
    public class RouteConfig
    {
        public readonly Type RequestType;
        public readonly Type ResultType;
        public readonly string Name;
        public readonly string BasePath;
        public readonly int SuccessCode = 200;
        public readonly bool Authenticate;
        public readonly List<string> AuthorizationPolicies;


        public RouteConfig(Type requestType)
        {
            RequestType = requestType;
            ResultType = DetermineResultTypes(requestType).FirstOrDefault();
            var api = requestType.GetAttribute<DtoRouteAttribute>();
            if (api == null) throw new Exception($"{requestType.FullName} type is not annotated with DtoRouteAttribute");

            var requestTypeName = requestType.Name;

            Name = api?.Name?.TrimStart('/')?.TrimEnd('/') ??
                   (requestTypeName.EndsWith("Dto")
                       ? requestTypeName.Remove(requestTypeName.Length - 3)
                       : requestTypeName);
            BasePath = api.BasePath.TrimStart('/').TrimEnd('/');
            Authenticate = api.Authenticate;
            AuthorizationPolicies = api.AuthorizationPolicies;
            SuccessCode = api.SuccessCode;
        } 

        public bool HasResult => ResultType != typeof(ResponselessRoute);

        private static IEnumerable<Type> DetermineResultTypes(Type type) =>
            from interfaceType in type.GetInterfaces()
            where interfaceType.IsGenericType
            where interfaceType.GetGenericTypeDefinition() == typeof(IRoutableDto<>)
            select interfaceType.GetGenericArguments()[0];
    }
}