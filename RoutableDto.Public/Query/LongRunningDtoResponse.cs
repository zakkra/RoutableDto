using System;


namespace RoutableDto.Public.Query
{
    /// <summary>
    /// Data echoed back on completion of long running operation  
    /// </summary>
    public class LongRunningDtoResponse
    {
        /// <summary>
        /// Value played back
        /// </summary>
        public string Echo { get; set; }
        /// <summary>
        /// Date time response was created
        /// </summary>
        public DateTime Created { get; set; }
    }
}
