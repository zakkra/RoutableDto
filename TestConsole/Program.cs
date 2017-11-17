using System;
using System.Collections.Generic;
using System.Linq;
using RoutableDto.Client;
using RoutableDto.Extensions;
using RoutableDto.Interfaces;
using RoutableDto.Public.Command;
using RoutableDto.Public.Query;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var di = new DummyDIContainer();
            var proxy = di.GetInstance<IRoutableDtoHandler<LongRunningDto, LongRunningDtoResponse>>();
            try
            {
                var r = proxy.HandleAsync(new LongRunningDto { Name = "my name is" });
                Console.WriteLine(r.Result.Echo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            var commandProxy = di.GetInstance<IRoutableDtoHandler<SampleCommandDto, ResponselessRoute>>();
            try
            {
                var r = commandProxy.HandleAsync(new SampleCommandDto
                {
                    Name = "my name is",
                    Options = new List<CommandOption>
                    {
                        new CommandOption{Name = "Option1", Description = "some option description"},
                        new CommandOption{Name = "Option2"}
                    }
                });
                Console.WriteLine("Command accepted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadKey();
        }



        public class DummyDIContainer
        {
            private Dictionary<Type, Type> dict;

            public DummyDIContainer()
            {
                Populate();
            }


            private void Populate()
            {
                var dtoAssemblies = new[] { typeof(LongRunningDto).Assembly };
                var types = dtoAssemblies.SelectMany(assembly =>
                    assembly.GetExportedTypes().Where(type => type.IsGenericTypeOf(typeof(IRoutableDto<>)) && type.GetAttribute<DtoRouteAttribute>() != null));

                var dtoConfing = types.Select(x => new RouteConfig(x));
                dict = dtoConfing.ToDictionary(x => typeof(IRoutableDtoHandler<,>).MakeGenericType(x.RequestType, x.ResultType) , x => typeof(Proxy<,>).MakeGenericType(x.RequestType, x.ResultType));
            }


            public T GetInstance<T>()
            {
                return (T)Activator.CreateInstance(dict[typeof(T)], new object[]{"http://localhost:6132"});

            }
        }
    }
}
