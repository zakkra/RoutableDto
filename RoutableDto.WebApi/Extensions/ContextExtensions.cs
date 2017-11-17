using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RoutableDto.WebApi.Extensions
{
    public static class ContextExtensions
    {
        public static async Task<string> ToJsonAsync(this Stream body)
        {
            using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        public static async Task WriteAsJsonAsync(this HttpResponse body, object dto)
        {
            var jsonString = JsonConvert.SerializeObject(dto);
         
            await body.WriteAsync(jsonString, Encoding.UTF8);
        }
    }
}
