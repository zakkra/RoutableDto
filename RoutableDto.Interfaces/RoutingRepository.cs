using System.Collections.Generic;
using System.Linq;

namespace RoutableDto.Interfaces
{
    public class RoutingRepository : IRoutingRepository
    {
        private readonly Dictionary<string,RouteConfig> dict = new Dictionary<string, RouteConfig>();

        public RoutingRepository() { }

        public RoutingRepository(Dictionary<string, RouteConfig> routingInfo)
        {
            dict = routingInfo.ToDictionary(x=>x.Key.ToLowerInvariant(), x=>x.Value);
        }

        public RouteConfig GetConfig(string path)
        {
            return dict[path.ToLowerInvariant()];
        }

        public void AddRoute(string path, RouteConfig routeConfig)
        {
            dict.Add(path.ToLowerInvariant(),routeConfig);
        }

        public bool PathSupported(string path)
        {
            return dict.ContainsKey(path.ToLowerInvariant());
        }

        public bool TryGetConfig(string path, out RouteConfig routeConfig)
        {
            return dict.TryGetValue(path.ToLowerInvariant(), out routeConfig);
        }
    }
}
