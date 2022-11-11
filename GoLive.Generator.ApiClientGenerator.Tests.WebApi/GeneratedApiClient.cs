// ReSharper disable All
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading;
using System.Text.Json.Serialization.Metadata;
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

        public async Task<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> Get(CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> _typeInfo = null)
        {
            var result = await _client.GetAsync($"/WeatherForecast/Get", cancellationToken: _token);
            return await result.Content.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token, jsonTypeInfo: _typeInfo);
        }

        public async Task<byte[]> GetBytes(CancellationToken _token = default)
        {
            var result = await _client.GetAsync($"/WeatherForecast/GetBytes", cancellationToken: _token);
            return await result.Content.ReadAsByteArrayAsync(cancellationToken: _token);
        }

        public async Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> GetSingle(int Id, CancellationToken _token = default, JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = null)
        {
            var result = await _client.GetAsync($"/WeatherForecast/GetSingle/{Id}", cancellationToken: _token);
            return await result.Content.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token, jsonTypeInfo: _typeInfo);
        }
    }
}// ReSharper disable All
