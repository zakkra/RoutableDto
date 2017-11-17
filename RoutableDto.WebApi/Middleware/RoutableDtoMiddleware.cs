using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using RoutableDto.Interfaces;
using RoutableDto.WebApi.Extensions;

namespace RoutableDto.WebApi.Middleware
{
    public class RoutableDtoMiddleware
    {
        private readonly RequestDelegate next;

        public RoutableDtoMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider services, IRoutingRepository routingRepo)//, IAuthorizationService authorizationService)
        {
            await next(context);


            if (!routingRepo.TryGetConfig(context.Request.Path, out RouteConfig routeConfig))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            if ("GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            dynamic dto;
            try
            {
                var queryData = await context.Request.Body.ToJsonAsync();
                dto = JsonConvert.DeserializeObject(queryData, routeConfig.RequestType);
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            //if (!IsAuthorized(authorizationService, context.User, dto, routeConfig))
            //{
            //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    return;
            //}

            try
            {
                var queryHandlerType = typeof(IRoutableDtoHandler<,>).MakeGenericType(routeConfig.RequestType, routeConfig.ResultType);
                var queryHandler = (dynamic)services.GetService(queryHandlerType);

                var result = await queryHandler.HandleAsync(dto);

                context.Response.StatusCode = routeConfig.SuccessCode;
                if (routeConfig.HasResult)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync((object)result);
                }

            }
            catch 
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

        }

        private static bool IsAuthorized(IAuthorizationService authorizationService, ClaimsPrincipal user, dynamic dto, RouteConfig routeConfig)
        {
            if (routeConfig.Authenticate && !user.Identity.IsAuthenticated) return false;

            if (!routeConfig.AuthorizationPolicies.Any()) return true;

            var authResults = routeConfig.AuthorizationPolicies.Select(x => authorizationService.AuthorizeAsync(user, dto, x));
            return authResults.All(x => x.Result.Succeeded);
        }
    }

    public static class RoutableDtoMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoutableDto(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoutableDtoMiddleware>();
        }
    }
}
