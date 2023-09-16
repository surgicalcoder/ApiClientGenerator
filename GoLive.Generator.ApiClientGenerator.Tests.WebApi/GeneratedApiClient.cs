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
            InheritingTwo = new InheritingTwoClient(client);
            InheritingUser2 = new InheritingUser2Client(client);
            User = new UserClient(client);
            WeatherForecast = new WeatherForecastClient(client);
        }

        public InheritingTwoClient InheritingTwo { get; }

        public InheritingUser2Client InheritingUser2 { get; }

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

    public class InheritingTwoClient
    {
        private readonly HttpClient _client;

        public InheritingTwoClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response> GetPagedApiTest(int Page  = 1, string Filter  = null, int PageSize  = 20, CancellationToken _token = default)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                queryString.Add("Filter", Filter.ToString());
            }
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingTwo/InheritingTwo/{Page}/{PageSize}", queryString), cancellationToken: _token);
            return new Response(result.StatusCode);
        }
         public string GetPagedApiTest_Url (int Page  = 1,string Filter  = null,int PageSize  = 20)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                queryString.Add("Filter", Filter.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingTwo/InheritingTwo/{Page}/{PageSize}", queryString), queryString);
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingTwo/", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
        }
         public string Get_Url ()
        {
            return $"/api/InheritingTwo/";
        }

        public async Task<Response<string>> GetUser(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingTwo/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser_Url (int Id )
        {
            return $"/api/InheritingTwo/";
        }

        public async Task<Response<int>> GetUser(string user , CancellationToken _token = default)
        {
            using var result = await _client.PostAsJsonAsync($"/api/InheritingTwo/", user, cancellationToken: _token);
            return new Response<int>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) 
                        ?? Task.FromResult<int>(default)));
        }
         public string GetUser_Url ()
        {
            return $"/api/InheritingTwo/";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , CancellationToken _token = default)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingTwo/", queryString), example, cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser2_Url (string Id ,string Id2 )
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingTwo/", queryString), queryString);
        }

        public async Task<Response<string>> OverrideTest(string Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingTwo/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string OverrideTest_Url (string Id )
        {
            return $"/api/InheritingTwo/";
        }
    }

    public class InheritingUser2Client
    {
        private readonly HttpClient _client;

        public InheritingUser2Client (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingUser2/", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
        }
         public string Get_Url ()
        {
            return $"/api/InheritingUser2/";
        }

        public async Task<Response<string>> GetUser(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingUser2/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser_Url (int Id )
        {
            return $"/api/InheritingUser2/";
        }

        public async Task<Response<int>> GetUser(string user , CancellationToken _token = default)
        {
            using var result = await _client.PostAsJsonAsync($"/api/InheritingUser2/", user, cancellationToken: _token);
            return new Response<int>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) 
                        ?? Task.FromResult<int>(default)));
        }
         public string GetUser_Url ()
        {
            return $"/api/InheritingUser2/";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , CancellationToken _token = default)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingUser2/", queryString), example, cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser2_Url (string Id ,string Id2 )
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/InheritingUser2/", queryString), queryString);
        }

        public async Task<Response<string>> OverrideTest(string Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/InheritingUser2/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string OverrideTest_Url (string Id )
        {
            return $"/api/InheritingUser2/";
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
            using var result = await _client.GetAsync($"/api/User/", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
        }
         public string Get_Url ()
        {
            return $"/api/User/";
        }

        public async Task<Response<string>> GetUser(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/User/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser_Url (int Id )
        {
            return $"/api/User/";
        }

        public async Task<Response<int>> GetUser(string user , CancellationToken _token = default)
        {
            using var result = await _client.PostAsJsonAsync($"/api/User/", user, cancellationToken: _token);
            return new Response<int>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) 
                        ?? Task.FromResult<int>(default)));
        }
         public string GetUser_Url ()
        {
            return $"/api/User/";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , CancellationToken _token = default)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/User/", queryString), example, cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string GetUser2_Url (string Id ,string Id2 )
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/User/", queryString), queryString);
        }

        public async Task<Response<string>> OverrideTest(string Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/User/", cancellationToken: _token);
            return new Response<string>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) 
                        ?? Task.FromResult<string?>(default)));
        }
         public string OverrideTest_Url (string Id )
        {
            return $"/api/User/";
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
            using var result = await _client.GetAsync($"/api/WeatherForecast/", cancellationToken: _token);
            return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token) 
                        ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)));
        }
         public string Get_Url ()
        {
            return $"/api/WeatherForecast/";
        }

        public async Task<Response> SecretUrl(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/_secretUrl", cancellationToken: _token);
            return new Response(result.StatusCode);
        }
         public string SecretUrl_Url ()
        {
            return $"/api/WeatherForecast/_secretUrl";
        }

        public async Task<Response> UrlWithParametersFromRoute(string Input1 , string Input2 , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}", cancellationToken: _token);
            return new Response(result.StatusCode);
        }
         public string UrlWithParametersFromRoute_Url (string Input1 ,string Input2 )
        {
            return $"/api/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}";
        }

        public async Task<Response> UrlWithParametersFromRoute2(string Input1 , string Input2 , string Input3 , CancellationToken _token = default)
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Input3))
            {
                queryString.Add("Input3", Input3.ToString());
            }
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}", queryString), cancellationToken: _token);
            return new Response(result.StatusCode);
        }
         public string UrlWithParametersFromRoute2_Url (string Input1 ,string Input2 ,string Input3 )
        {
            Dictionary<string, string> queryString=new();
            if (!string.IsNullOrWhiteSpace(Input3))
            {
                queryString.Add("Input3", Input3.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}", queryString), queryString);
        }

        public async Task<Response<byte[]>> GetBytes(CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/", cancellationToken: _token);
            return new Response<byte[]>(
                result.StatusCode,
                await (result.Content?.ReadAsByteArrayAsync() 
                        ?? Task.FromResult<byte[]?>(default)));
        }
         public string GetBytes_Url ()
        {
            return $"/api/WeatherForecast/";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingle(int Id , CancellationToken _token = default)
        {
            using var result = await _client.GetAsync($"/api/WeatherForecast/", cancellationToken: _token);
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(
                result.StatusCode,
                await (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) 
                        ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
        }
         public string GetSingle_Url (int Id )
        {
            return $"/api/WeatherForecast/";
        }
    }
}
// ReSharper disable All
