using RoutableDto.Interfaces;


namespace RoutableDto.Public.Query
{
    [DtoRoute("query")]
    public class LongRunningDto: IRoutableDto<LongRunningDtoResponse>
    {
        public string Name { get; set; }
    }
}
