using System;
using System.Threading.Tasks;
using RoutableDto.Interfaces;
using RoutableDto.Public.Query;

namespace RoutableDto.Handlers.Query
{
    public class LongRunningDtoHandler:IRoutableDtoHandler<LongRunningDto, LongRunningDtoResponse>
    {
        public async Task<LongRunningDtoResponse> HandleAsync(LongRunningDto dto)
        {
            await  Task.Delay(TimeSpan.FromSeconds(2));
            return new LongRunningDtoResponse{Echo = dto.Name, Created = DateTime.Now};
        }
    }
}
