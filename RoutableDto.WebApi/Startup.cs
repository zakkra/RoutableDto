using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RoutableDto.Interfaces;
using RoutableDto.WebApi.Middleware;
using RoutableDto.WebApi.Extensions;
using RoutableDto.Handlers.Query;
using RoutableDto.Public.Query;
using System.Linq;
using RoutableDto.Extensions;

namespace RoutableDto.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IRoutableDtoHandler<,>), typeof(LongRunningDtoHandler).Assembly);
            services.AddSingleton<IRoutingRepository>(new RoutingRepository(GenerateRoutingConfig()));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseRoutableDto();
        }

        private static Dictionary<string, RouteConfig> GenerateRoutingConfig()
        {
            var dtoAssemblies = new[] { typeof(LongRunningDto).Assembly };
            var types = dtoAssemblies.SelectMany(assembly =>
                assembly.GetExportedTypes().Where(type => type.IsGenericTypeOf(typeof(IRoutableDto<>)) && type.GetAttribute<DtoRouteAttribute>()!=null));

            var dtoConfing = types.Select(x => new RouteConfig(x));
            return dtoConfing.ToDictionary(x => $"/{x.BasePath}/{x.Name}", x => x);


        }
    }
}
