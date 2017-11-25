using System.Collections.Generic;

namespace RoutableDto.Interfaces
{
    public interface IRoutingRepository
    {
        bool PathSupported(string path);
        bool TryGetConfig(string path, out RouteConfig routeConfig);
        RouteConfig GetConfig(string path);
        void AddRoute(string path, RouteConfig routeConfig);
        IEnumerable<(string Path, RouteConfig RouteConfig)> Routes { get; }
    }
}
