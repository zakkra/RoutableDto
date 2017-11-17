using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutableDto.Interfaces;

namespace RoutableDto.Client
{
    public class Proxy<TRequest, TResponse> : IRoutableDtoHandler<TRequest, TResponse> where TRequest: IRoutableDto<TResponse>
    {
        HttpClient httpClient = new HttpClient();
        private static readonly RouteConfig routeConfig;

        static Proxy()
        {
            routeConfig = new RouteConfig(typeof(TRequest));
        }

        public Proxy(string baseAddress)
        {
            httpClient.BaseAddress = new Uri($"{baseAddress.Trim('/')}");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<TResponse> HandleAsync(TRequest dto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{routeConfig.BasePath}/{routeConfig.Name}", stringContent).ConfigureAwait(false);

            if ((int) response.StatusCode == routeConfig.SuccessCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return (TResponse)(JsonConvert.DeserializeObject(json, routeConfig.ResultType));
            }
            throw new Exception($"Unexpected code {response.StatusCode.ToString()} returned by the service");
        }
    }
}
