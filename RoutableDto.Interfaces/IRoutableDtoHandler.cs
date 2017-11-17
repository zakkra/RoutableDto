
using System.Threading.Tasks;

namespace RoutableDto.Interfaces
{
    public interface IRoutableDtoHandler<TRoutableDto, TResponse> where TRoutableDto:IRoutableDto<TResponse>
    {
        Task<TResponse> HandleAsync(TRoutableDto dto);
    }
}
