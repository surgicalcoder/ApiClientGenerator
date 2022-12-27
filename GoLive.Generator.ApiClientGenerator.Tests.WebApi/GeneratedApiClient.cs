// ReSharper disable All
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading;
using System.Net;
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

    public class Response
    {
        public Response()
        {
        }

        public Response(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; internal set; }

        public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
    }

    public class Response<T> : Response
    {
        public Response()
        {
        }

        public Response(HttpStatusCode statusCode, T data) : base(statusCode)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class WeatherForecastClient
    {
        private readonly HttpClient _client;
        public WeatherForecastClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<Task<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>>> Get(CancellationToken _token = default)
        {
            var result = await _client.GetAsync($"/api/WeatherForecast/Get", cancellationToken: _token);
            return new Response<Task<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>>(result.StatusCode, result.Content.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>());
        }

        public async Task<Response> SecretUrl(CancellationToken _token = default)
        {
            var result = await _client.GetAsync($"/api_secretUrl", cancellationToken: _token);
            return new Response(result.StatusCode);
        }

        public async Task<Response<Task<byte[]>>> GetBytes(CancellationToken _token = default)
        {
            var result = await _client.GetAsync($"/api/WeatherForecast/GetBytes", cancellationToken: _token);
            return new Response<Task<byte[]>>(result.StatusCode, result.Content.ReadAsByteArrayAsync());
        }

        public async Task<Response<Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>> GetSingle(int Id, CancellationToken _token = default)
        {
            var result = await _client.GetAsync($"/api/WeatherForecast/GetSingle/{Id}", cancellationToken: _token);
            return new Response<Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(result.StatusCode, result.Content.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>());
        }
    }
}// ReSharper disable All
