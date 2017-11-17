# Routable Dto

The goal of this project is to provide ASP.NET Core 2 Api developers with an easy and clean way of setting up and using controlerless Api routing mechanism.

It allows dto classes that are used as inputs into Api methods, to drive their own routing, responses, response codes and security. 
Each dto is linked to a single execution handler that is responsible for processing of that particular dto type. This enforces single responsibility principal and provides all of its benefits. 

In order to become routable, dto class needs 2 things:
1. Implement ``` IRoutableDto<T> ``` interface
2. Be annotated with ``` DtoRouteAttribute ```

Dto Example
===============

``` c#
    [DtoRoute("query")]
    public class LongRunningDto: IRoutableDto<LongRunningDtoResponse>
    {
        public string Name { get; set; }
    }

```
The ```DtoRoute``` attribute must at very least provide base path for the route. 
In the above case, the target route will be /query/LongRunning but it could be changed within the attribute by setting different Name value. 

The ```IRoutableDto``` interface defines no methods or properties and is only concerned with capturing the return type for this route. Classes that implement the interface without being annotated with the DtoRoute attribute can still be used internally (e.g. Dto handlers can use other handler) but such dtos will not be publicly exposed via Api. 

Routes that return no json response should be defined as dtos that implement ```IRoutableDto<ResponselessRoute>```

Every dto must have one (and only one) handler that is charged with processing calls to its route

DtoHandler Example
===============

``` c#
    public class LongRunningDtoHandler:IRoutableDtoHandler<LongRunningDto, LongRunningDtoResponse>
    {
        public async Task<LongRunningDtoResponse> HandleAsync(LongRunningDto dto)
        {
            await  Task.Delay(TimeSpan.FromSeconds(2));
            return new LongRunningDtoResponse{Echo = dto.Name, Created = DateTime.Now};
        }
    }
```

Dto class and its handler are automatically linked by matching generic types of the handler to dto type and its generic return type.

As ```RoutableDtoMiddleware``` expects dto data in the body of the request, it actively rejects all GET calls. 

At the moment only Json format is accepted in requests and only Json format is returned in response.

Proxy client is also part of the solution and can be used within c# clients who need to talk to the Api. The TestConole project demonstrates how this proxy can be used.



Many thanks to [Steven](https://stackoverflow.com/users/264697/steven) for providing inspiration with his work on [wcf services](https://cuttingedge.it/blogs/steven/pivot/entry.php?id=92) 

