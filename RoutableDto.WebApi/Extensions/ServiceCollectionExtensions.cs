using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RoutableDto.Extensions;

namespace RoutableDto.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddTransient(this IServiceCollection services, Type type, params Assembly[] assemblies)
        {
            if (type.IsOpenGenericType())
            {
                services.Scan(scan => scan
                    .FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(type))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            }
            else
            {
                services.Scan(scan => scan
                    .FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(type))
                    .As(type)
                    .WithTransientLifetime());
            }


            return services;
        }

        public static IServiceCollection AddScoped(this IServiceCollection services, Type type, params Assembly[] assemblies)
        {
            if (type.IsOpenGenericType())
            {
                services.Scan(scan => scan
                    .FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(type))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            }
            else
            {
                services.Scan(scan => scan
                    .FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(type))
                    .As(type)
                    .WithScopedLifetime());
            }
                
            return services;
        }
    }
}
