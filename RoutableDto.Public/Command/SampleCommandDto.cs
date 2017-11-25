using System.Collections.Generic;
using RoutableDto.Interfaces;

namespace RoutableDto.Public.Command
{
    /// <summary>
    /// Sample command call
    /// </summary>
    [DtoRoute("command", Name = "RunSample", SuccessCode = 202, Authenticate =  true)]
    public class SampleCommandDto: IRoutableDto<ResponselessRoute>
    {
        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Command options
        /// </summary>
        public List<CommandOption> Options { get; set; }
    }

    /// <summary>
    /// Inner command options
    /// </summary>
    public class CommandOption
    {
        /// <summary>
        /// Option name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Option Description
        /// </summary>
        public string Description { get; set; }
    }
}
