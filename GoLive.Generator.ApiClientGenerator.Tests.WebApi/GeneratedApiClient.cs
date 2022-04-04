// ReSharper disable All
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using GoLive.Generator.ApiClientGenerator;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        public ApiClient(HttpClient client)
        {
            _client = client;
            WeatherForecast = new WeatherForecastClient(client);
        }

        public WeatherForecastClient WeatherForecast { get; }
    }

    public class WeatherForecastClient
    {
        private readonly HttpClient _client;
        public WeatherForecastClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> Get()
        {
            var result = await _client.GetAsync($"/WeatherForecast/Get");
            return await result.Content.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>();
        }

        public async Task<byte[]> GetBytes()
        {
            var result = await _client.GetAsync($"/WeatherForecast/GetBytes");
            return await result.Content.ReadAsByteArrayAsync();
        }

        public async Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> GetSingle(int Id)
        {
            var result = await _client.GetAsync($"/WeatherForecast/GetSingle/{Id}");
            return await result.Content.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>();
        }
    }
}// ReSharper disable All
