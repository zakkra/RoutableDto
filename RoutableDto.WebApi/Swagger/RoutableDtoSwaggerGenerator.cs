using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RoutableDto.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RoutableDto.WebApi.Swagger
{
    public class RoutableDtoSwaggerGenerator : IDocumentFilter
    {
        private readonly IRoutingRepository routingRepo;

        public RoutableDtoSwaggerGenerator(IRoutingRepository routingRepo)
        {
            this.routingRepo = routingRepo;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            Enrich(swaggerDoc, context.SchemaRegistry);
        }

        private void Enrich(SwaggerDocument swagger, ISchemaRegistry schemaRegistry)
        {
            var paths = routingRepo.Routes.ToDictionary(d =>$"/{d.RouteConfig.BasePath}/{d.RouteConfig.Name}", d => CreatePathItem(d.RouteConfig, schemaRegistry));
            swagger.Paths = paths;
        }

        private PathItem CreatePathItem(RouteConfig routeConfig, ISchemaRegistry schemaRegistry)
        {
            var pathItem = new PathItem();
        
            pathItem.Post = CreateOperation(routeConfig, schemaRegistry);
            return pathItem;
        }

        private Operation CreateOperation(RouteConfig routeConfig, ISchemaRegistry schemaRegistry)
        {
            var parameters = new[] { CreateParameter(routeConfig, schemaRegistry) };

            var responses = new[] { new ApiResponseType { StatusCode = routeConfig.SuccessCode } }
                .ToDictionary(
                    apiResponseType => apiResponseType.StatusCode.ToString(),
                    apiResponseType => CreateResponse(routeConfig, schemaRegistry)
                 );

            var schema = schemaRegistry.Definitions.FirstOrDefault(x=>x.Key == routeConfig.RequestType.Name);

            var operation = new Operation
            {
                Tags = new[] {routeConfig.BasePath},
                OperationId = $"{routeConfig.BasePath.Replace("/", "")}-{routeConfig.Name}",
                Consumes = new List<string> { @"application/json" },
                Produces = new List<string> { @"application/json" },
                Parameters = parameters.Any() ? parameters : null, // parameters can be null but not empty
                Responses = responses,
                Deprecated = false,
                Summary = schema.Value.Description
            };
            return operation;
        }

        private IParameter CreateParameter(RouteConfig routeConfig, ISchemaRegistry schemaRegistry)
        {
            var schema = schemaRegistry.GetOrRegister(routeConfig.RequestType);

            return new BodyParameter
            {
                Name = "Dto",
                Schema = schema
            };

        }

        private Response CreateResponse(RouteConfig apiResponseType, ISchemaRegistry schemaRegistry)
        {
            return new Response
            {
                Description = apiResponseType.HasResult
                    ? apiResponseType.ResultType.FullName
                    : "No response data",
                Schema = apiResponseType.HasResult
                    ? schemaRegistry.GetOrRegister(apiResponseType.ResultType)
                    : null

            };
        }
    }
}
