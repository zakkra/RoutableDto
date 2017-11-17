using System;
using System.Collections.Generic;
using System.Text;

namespace RoutableDto.Interfaces
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DtoRouteAttribute : Attribute
    {
        public DtoRouteAttribute(string basePath)
        {
            BasePath = basePath;
            SuccessCode = 200;
        }

        public string Name { get; set; }
        public string BasePath { get; }
        public int SuccessCode { get; set; }
        public bool Authenticate { get; set; }
        public List<string> AuthorizationPolicies{ get; set; } =new List<string>();
    }

}
