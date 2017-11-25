using RoutableDto.Interfaces;


namespace RoutableDto.Public.Query
{
    /// <summary>
    /// Sample long running operation
    /// </summary>
    [DtoRoute("query")]
    public class LongRunningDto: IRoutableDto<LongRunningDtoResponse>
    {
        /// <summary>
        /// Test name
        /// </summary>
        public string Name { get; set; }
    }
}
