using System;
using System.Threading.Tasks;
using RoutableDto.Interfaces;
using RoutableDto.Public.Command;

namespace RoutableDto.Handlers.Command
{
    public class SampleCommandDtoHandler : IRoutableDtoHandler<SampleCommandDto, ResponselessRoute>
    {
        public async Task<ResponselessRoute> HandleAsync(SampleCommandDto dto)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return new ResponselessRoute();
        }
    }
}
