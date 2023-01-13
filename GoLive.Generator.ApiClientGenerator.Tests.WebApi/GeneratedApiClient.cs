// ReSharper disable All
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using GoLive.Generator.ApiClientGenerator;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated
{

    public class ApiClient
    {
        public ApiClient(HttpClient client)
        {
            User = new UserClient(client);
            WeatherForecast = new WeatherForecastClient(client);
        }

        public UserClient User { get; }

        public WeatherForecastClient WeatherForecast { get; }
    }
    public class Response
    {
        public Response() {}
        public Response(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; }
        public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
    }
    public class Response<T> : Response
    {
        public Response() {}
        public Response(HttpStatusCode statusCode, T? data) : base(statusCode)
        {
            Data = data;
        }
        public T? Data { get; }
        
        public T SuccessData => Success ? Data ?? throw new NullReferenceException("Response had an empty body!")
                                        : throw new InvalidOperationException("Request was not successful!");
        public bool TryGetSuccessData([NotNullWhen(true)] out T? data)
        {
            data = Data;
            return Success && data is not null;
        }
    }

    public class UserClient
    {
        private readonly HttpClient _client;

        public UserClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/User/Get", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
        }

        public async Task<Response<string>> GetUser(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/User/GetUser/{Id}", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }

        public async Task<Response<int>> GetUser(string user , CancellationToken _token = default)
        {
            using var result = await _client.PostAsJsonAsync($"/api/User/GetUser", user, cancellationToken: _token);
            return new Response<int>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) 
                        ?? Task.FromResult<int>(default)));
        }
    }

    public class WeatherForecastClient
    {
        private readonly HttpClient _client;

        public WeatherForecastClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>> Get(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/Get", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)));
        }

        public async Task<Response> SecretUrl(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api_secretUrl", cancellationToken: _token);
            return new Response(result.StatusCode);
        }

        public async Task<Response<byte[]>> GetBytes(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/GetBytes", cancellationToken: _token);
            return new Response<byte[]>(
                result.StatusCode,
                await (result.Content?.ReadAsByteArrayAsync() 
                        ?? Task.FromResult<byte[]?>(default)));
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingle(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/GetSingle/{Id}", cancellationToken: _token);
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) 
                        ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
        }
    }
}
// ReSharper disable All
