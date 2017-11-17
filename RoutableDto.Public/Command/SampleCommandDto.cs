using System.Collections.Generic;
using RoutableDto.Interfaces;

namespace RoutableDto.Public.Command
{
    [DtoRoute("command", Name = "RunSample", SuccessCode = 202, Authenticate =  true)]
    public class SampleCommandDto: IRoutableDto<ResponselessRoute>
    {
        public string Name { get; set; }
        public List<CommandOption> Options { get; set; }
    }

    public class CommandOption
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
