// ReSharper disable All
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;
using GoLive.Generator.ApiClientGenerator;

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated
{

    public class ApiClient
    {
        public ApiClient(HttpClient client)
        {
            InheritingTwo = new InheritingTwoClient(client);
            InheritingUser2 = new InheritingUser2Client(client);
            NonApi = new NonApiClient(client);
            User = new UserClient(client);
            WeatherForecast = new WeatherForecastClient(client);
            YetAnother = new YetAnotherClient(client);
        }

        public InheritingTwoClient InheritingTwo { get; }

        public InheritingUser2Client InheritingUser2 { get; }

        public NonApiClient NonApi { get; }

        public UserClient User { get; }

        public WeatherForecastClient WeatherForecast { get; }

        public YetAnotherClient YetAnother { get; }
    }
    public class EmptyBodyException : ApplicationException
    {
        public EmptyBodyException(int statusCode, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
        public int StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }
    }
    public class UnsuccessfulException : ApplicationException
    {
        public UnsuccessfulException(int statusCode, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
        public int StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }
    }
    public class Response
    {
        public HttpResponseHeaders Headers {get; set;}
        public Response(HttpResponseHeaders headers)
        {
            this.Headers = headers;
            if (headers != null)
            {
                _populateValuesFromHeaders();
            }
        }
        public Response(HttpStatusCode statusCode, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            this.Headers = headers;
            if (headers != null)
            {
                _populateValuesFromHeaders();
            }
        }
        public void _populateValuesFromHeaders()
        {
            if (Headers != null && Headers.Contains("X-Correlation-Id") )
            {
                CorrelationId = Headers.GetValues("X-Correlation-Id");
            }
        }
        public HttpStatusCode StatusCode { get; }
        public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
        public IEnumerable<string> CorrelationId {get; set; } = [];
    }
    public class Response<T> : Response
    {
        public Response(HttpResponseHeaders headers) : base(headers) {}
        public Response(HttpStatusCode statusCode, HttpResponseHeaders headers, Task<T?> data) : base(statusCode, headers)
        {
            Data = data;
            this.Headers = headers;
            if (headers != null)
            {
                _populateValuesFromHeaders();
            }
        }
        public Task<T?> Data { get; }
        
        public Task<T> SuccessData => Success ? Data ?? throw new  EmptyBodyException((int)StatusCode, Headers)
                                        : throw new UnsuccessfulException((int)StatusCode, Headers);
        public bool TryGetSuccessData([NotNullWhen(true)] out Task<T?> data)
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

        public async Task<Response> GetPagedApiTest(int Page  = 1, string Filter  = null, int PageSize  = 20, QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                queryString.Add("Filter", Filter.ToString());
            }
            using var result = await _client.GetAsync($"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string GetPagedApiTest_Url (int Page  = 1,string Filter  = null,int PageSize  = 20, QueryString queryString = default)
        {
            queryString.Add("Filter", Filter.ToString());
            return $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}";
        }

        public async Task<Response> GetApiTest2(int Page  = 1, QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/ThisIsTestTwo/{Page}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string GetApiTest2_Url (int Page  = 1, QueryString queryString = default)
        {
            return $"/ThisIsTestTwo/{Page}{queryString}";
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
            else
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
        }
        public string Get_Url (QueryString queryString = default)
        {
            return $"/InheritingTwo/{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (int Id , QueryString queryString = default)
        {
            return $"/InheritingTwo/{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingTwo/{queryString}", user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)));
            }
            else
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            return $"/InheritingTwo/{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingTwo/{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , QueryString queryString = default)
        {
            queryString.Add("Id2", Id2.ToString());
            return $"/InheritingTwo/{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (string Id , QueryString queryString = default)
        {
            return $"/InheritingTwo/{queryString}";
        }
    }

    public class InheritingUser2Client
    {
        private readonly HttpClient _client;

        public InheritingUser2Client (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            using var result = await _client.GetAsync($"/InheritingUser2/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
            else
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
        }
        public string Get_Url (QueryString queryString = default)
        {
            return $"/InheritingUser2/{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingUser2/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (int Id , QueryString queryString = default)
        {
            return $"/InheritingUser2/{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingUser2/{queryString}", user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)));
            }
            else
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            return $"/InheritingUser2/{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingUser2/{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , QueryString queryString = default)
        {
            queryString.Add("Id2", Id2.ToString());
            return $"/InheritingUser2/{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingUser2/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (string Id , QueryString queryString = default)
        {
            return $"/InheritingUser2/{queryString}";
        }
    }

    public class NonApiClient
    {
        private readonly HttpClient _client;

        public NonApiClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response> TestModelBinderDifferentNameUnderId(System.String Id , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestModelBinderDifferentNameUnderId_Url (System.String Id , QueryString queryString = default)
        {
            return $"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}";
        }

        public async Task<Response> TestModelBinderDifferentNameUnderId3(System.String Id , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestModelBinderDifferentNameUnderId3_Url (System.String Id , QueryString queryString = default)
        {
            return $"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}";
        }
    }

    public class UserClient
    {
        private readonly HttpClient _client;

        public UserClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            using var result = await _client.GetAsync($"/User/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
            else
            {
                return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)));
            }
        }
        public string Get_Url (QueryString queryString = default)
        {
            return $"/User/{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/User/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (int Id , QueryString queryString = default)
        {
            return $"/User/{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/User/{queryString}", user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)));
            }
            else
            {
                return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            return $"/User/{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/User/{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , QueryString queryString = default)
        {
            queryString.Add("Id2", Id2.ToString());
            return $"/User/{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/User/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (string Id , QueryString queryString = default)
        {
            return $"/User/{queryString}";
        }
    }

    public class WeatherForecastClient
    {
        private readonly HttpClient _client;

        public WeatherForecastClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response> TestIgnoreGenericParmaeter(string optionNotRemoved , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(optionNotRemoved))
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreGenericParmaeter_Url (string optionNotRemoved , QueryString queryString = default)
        {
            queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TestIgnoreNormalParameter(string optionNotRemoved , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(optionNotRemoved))
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreNormalParameter_Url (string optionNotRemoved , QueryString queryString = default)
        {
            queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TestIgnoreWithCustomAttribute(string optionNotRemoved , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(optionNotRemoved))
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreWithCustomAttribute_Url (string optionNotRemoved , QueryString queryString = default)
        {
            queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TestModelBinderDifferentName(System.String OtherName , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(OtherName))
            {
                queryString.Add("OtherName", OtherName.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestModelBinderDifferentName_Url (System.String OtherName , QueryString queryString = default)
        {
            queryString.Add("OtherName", OtherName.ToString());
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TestRemapTypeWithAnotherType(System.TimeSpan option , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", option, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestRemapTypeWithAnotherType_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TestRemapTypeWithAnotherType2(System.String option , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast/{queryString}", option, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestRemapTypeWithAnotherType2_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)));
            }
            else
            {
                return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)));
            }
        }
        public string Get_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> SecretUrl(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast/_secretUrl{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string SecretUrl_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/_secretUrl{queryString}";
        }

        public async Task<Response> UrlWithParametersFromRoute(string Input1 , string Input2 , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string UrlWithParametersFromRoute_Url (string Input1 ,string Input2 , QueryString queryString = default)
        {
            return $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}";
        }

        public async Task<Response> UrlWithParametersFromRoute2(string Input1 , string Input2 , string Input3 , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(Input3))
            {
                queryString.Add("Input3", Input3.ToString());
            }
            using var result = await _client.GetAsync($"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string UrlWithParametersFromRoute2_Url (string Input1 ,string Input2 ,string Input3 , QueryString queryString = default)
        {
            queryString.Add("Input3", Input3.ToString());
            return $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}";
        }

        public async Task<Response<byte[]>> GetBytes(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast/{queryString}", cancellationToken: _token);
            return new Response<byte[]>(
                result.StatusCode,
                result.Headers,
                (result.Content?.ReadAsByteArrayAsync()
                        ?? Task.FromResult<byte[]?>(default)));
        }
        public string GetBytes_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingle(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
            else
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
        }
        public string GetSingle_Url (int Id , QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingleFromServiceExample(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast/{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
            else
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
        }
        public string GetSingleFromServiceExample_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> FormUploadTest1(System.Net.Http.MultipartFormDataContent multiPartContent, QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsync($"/WeatherForecast/{queryString}", multiPartContent, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string FormUploadTest1_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> FormUploadTest2(System.Net.Http.MultipartFormDataContent? multiPartContent, QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsync($"/WeatherForecast/{queryString}", multiPartContent, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string FormUploadTest2_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }

        public async Task<Response> TaskIssue(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast/{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TaskIssue_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/{queryString}";
        }
    }

    public class YetAnotherClient
    {
        private readonly HttpClient _client;

        public YetAnotherClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<string>> YetAnotherTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.PostAsJsonAsync($"/api/YetAnother/YetAnotherTest/{Id}{queryString}", new {}, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string YetAnotherTest_Url (string Id , QueryString queryString = default)
        {
            return $"/api/YetAnother/YetAnotherTest/{Id}{queryString}";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>> JSONDynamicTestDynamic(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>;
            }
            using var result = await _client.GetAsync($"/api/YetAnother/JSONDynamicTestDynamic{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest?>(default)));
            }
            else
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest?>(default)));
            }
        }
        public string JSONDynamicTestDynamic_Url (QueryString queryString = default)
        {
            return $"/api/YetAnother/JSONDynamicTestDynamic{queryString}";
        }
    }
}
// JSON Source Generator
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,AllowTrailingCommas = true)]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<string>))]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>))]
[JsonSerializable(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast))]
[JsonSerializable(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
// JSON Source Generator
// ReSharper disable All
