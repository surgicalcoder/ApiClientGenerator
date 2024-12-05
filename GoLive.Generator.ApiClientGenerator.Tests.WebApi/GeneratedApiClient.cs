// ReSharper disable All
/*
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
            if (Page != default)
            {
                queryString = queryString.Add("Page", Page.ToString());
            }
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                queryString = queryString.Add("Filter", Filter.ToString());
            }
            if (PageSize != default)
            {
                queryString = queryString.Add("PageSize", PageSize.ToString());
            }
            using var result = await _client.GetAsync($"/InheritingTwo/InheritingTwo/{Page}/{PageSize}?Action=GetPagedApiTest&Filter=%7BFilter%7D{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string GetPagedApiTest_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Page", Page.ToString());
            queryString = queryString.Add("Filter", Filter.ToString());
            queryString = queryString.Add("PageSize", PageSize.ToString());
            return $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}?Action=GetPagedApiTest&Filter=%7BFilter%7D{queryString}";
        }

        public async Task<Response> GetApiTest2(int Page  = 1, QueryString queryString = default, CancellationToken _token = default )
        {
            if (Page != default)
            {
                queryString = queryString.Add("Page", Page.ToString());
            }
            using var result = await _client.GetAsync($"/ThisIsTestTwo/{Page}?Controller=InheritingTwo&Action=GetApiTest2{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string GetApiTest2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Page", Page.ToString());
            return $"/ThisIsTestTwo/{Page}?Controller=InheritingTwo&Action=GetApiTest2{queryString}";
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo?Action=Get{queryString}", cancellationToken: _token);
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
            return $"/InheritingTwo?Action=Get{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo?Action=GetUser&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/InheritingTwo?Action=GetUser&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingTwo?Action=GetUser&user=%7Buser%7D{queryString}", user, cancellationToken: _token);
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
            queryString = queryString.Add("user", user.ToString());
            return $"/InheritingTwo?Action=GetUser&user=%7Buser%7D{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString = queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingTwo?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            queryString = queryString.Add("Id2", Id2.ToString());
            queryString = queryString.Add("example", example.ToString());
            return $"/InheritingTwo?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingTwo?Action=OverrideTest&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/InheritingTwo?Action=OverrideTest&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<string>> GetUser4(int Id3 , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (Id3 != default)
            {
                queryString = queryString.Add("Id3", Id3.ToString());
            }
            using var result = await _client.GetAsync($"/InheritingTwo?Action=GetUser4&Id3=%7BId3%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser4_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
            return $"/InheritingTwo?Action=GetUser4&Id3=%7BId3%7D{queryString}";
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
            using var result = await _client.GetAsync($"/InheritingUser2?Action=Get{queryString}", cancellationToken: _token);
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
            return $"/InheritingUser2?Action=Get{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingUser2?Action=GetUser&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/InheritingUser2?Action=GetUser&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingUser2?Action=GetUser&user=%7Buser%7D{queryString}", user, cancellationToken: _token);
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
            queryString = queryString.Add("user", user.ToString());
            return $"/InheritingUser2?Action=GetUser&user=%7Buser%7D{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString = queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/InheritingUser2?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            queryString = queryString.Add("Id2", Id2.ToString());
            queryString = queryString.Add("example", example.ToString());
            return $"/InheritingUser2?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/InheritingUser2?Action=OverrideTest&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/InheritingUser2?Action=OverrideTest&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<string>> GetUser4(int Id3 , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (Id3 != default)
            {
                queryString = queryString.Add("Id3", Id3.ToString());
            }
            using var result = await _client.GetAsync($"/InheritingUser2?Action=GetUser4&Id3=%7BId3%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser4_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
            return $"/InheritingUser2?Action=GetUser4&Id3=%7BId3%7D{queryString}";
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
            using var result = await _client.GetAsync($"/User?Action=Get{queryString}", cancellationToken: _token);
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
            return $"/User?Action=Get{queryString}";
        }

        public async Task<Response<string>> GetUser(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/User?Action=GetUser&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/User?Action=GetUser&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<int>> GetUser(string user , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            using var result = await _client.PostAsJsonAsync($"/User?Action=GetUser&user=%7Buser%7D{queryString}", user, cancellationToken: _token);
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
            queryString = queryString.Add("user", user.ToString());
            return $"/User?Action=GetUser&user=%7Buser%7D{queryString}";
        }

        public async Task<Response<string>> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (!string.IsNullOrWhiteSpace(Id2))
            {
                queryString = queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/User?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}", example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            queryString = queryString.Add("Id2", Id2.ToString());
            queryString = queryString.Add("example", example.ToString());
            return $"/User?Action=GetUser2&Id=%7BId%7D&Id2=%7BId2%7D&example=%7Bexample%7D{queryString}";
        }

        public async Task<Response<string>> OverrideTest(string Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            using var result = await _client.GetAsync($"/User?Action=OverrideTest&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string OverrideTest_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/User?Action=OverrideTest&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<string>> GetUser4(int Id3 , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            if (Id3 != default)
            {
                queryString = queryString.Add("Id3", Id3.ToString());
            }
            using var result = await _client.GetAsync($"/User?Action=GetUser4&Id3=%7BId3%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)));
            }
            else
            {
                return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)));
            }
        }
        public string GetUser4_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
            return $"/User?Action=GetUser4&Id3=%7BId3%7D{queryString}";
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
                queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestIgnoreGenericParmaeter&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreGenericParmaeter_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast?Action=TestIgnoreGenericParmaeter&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}";
        }

        public async Task<Response> TestIgnoreNormalParameter(string optionNotRemoved , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(optionNotRemoved))
            {
                queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestIgnoreNormalParameter&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreNormalParameter_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast?Action=TestIgnoreNormalParameter&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}";
        }

        public async Task<Response> TestIgnoreWithCustomAttribute(string optionNotRemoved , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(optionNotRemoved))
            {
                queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestIgnoreWithCustomAttribute&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestIgnoreWithCustomAttribute_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            return $"/WeatherForecast?Action=TestIgnoreWithCustomAttribute&optionNotRemoved=%7BoptionNotRemoved%7D{queryString}";
        }

        public async Task<Response> TestModelBinderDifferentName(System.String OtherName , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(OtherName))
            {
                queryString = queryString.Add("OtherName", OtherName.ToString());
            }
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestModelBinderDifferentName&OtherName=%7BOtherName%7D{queryString}", new {}, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestModelBinderDifferentName_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("OtherName", OtherName.ToString());
            return $"/WeatherForecast?Action=TestModelBinderDifferentName&OtherName=%7BOtherName%7D{queryString}";
        }

        public async Task<Response> TestRemapTypeWithAnotherType(System.TimeSpan option , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestRemapTypeWithAnotherType&option=%7Boption%7D{queryString}", option, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestRemapTypeWithAnotherType_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("option", option.ToString());
            return $"/WeatherForecast?Action=TestRemapTypeWithAnotherType&option=%7Boption%7D{queryString}";
        }

        public async Task<Response> TestRemapTypeWithAnotherType2(System.String option , QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsJsonAsync($"/WeatherForecast?Action=TestRemapTypeWithAnotherType2&option=%7Boption%7D{queryString}", option, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TestRemapTypeWithAnotherType2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("option", option.ToString());
            return $"/WeatherForecast?Action=TestRemapTypeWithAnotherType2&option=%7Boption%7D{queryString}";
        }

        public async Task<Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>> Get(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast?Action=Get{queryString}", cancellationToken: _token);
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
            return $"/WeatherForecast?Action=Get{queryString}";
        }

        public async Task<Response> SecretUrl(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast/_secretUrl?Action=SecretUrl{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string SecretUrl_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast/_secretUrl?Action=SecretUrl{queryString}";
        }

        public async Task<Response> UrlWithParametersFromRoute(string Input1 , string Input2 , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(Input1))
            {
                queryString = queryString.Add("Input1", Input1.ToString());
            }
            if (!string.IsNullOrWhiteSpace(Input2))
            {
                queryString = queryString.Add("Input2", Input2.ToString());
            }
            using var result = await _client.GetAsync($"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}?Action=UrlWithParametersFromRoute{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string UrlWithParametersFromRoute_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Input1", Input1.ToString());
            queryString = queryString.Add("Input2", Input2.ToString());
            return $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}?Action=UrlWithParametersFromRoute{queryString}";
        }

        public async Task<Response> UrlWithParametersFromRoute2(string Input1 , string Input2 , string Input3 , QueryString queryString = default, CancellationToken _token = default )
        {
            if (!string.IsNullOrWhiteSpace(Input1))
            {
                queryString = queryString.Add("Input1", Input1.ToString());
            }
            if (!string.IsNullOrWhiteSpace(Input2))
            {
                queryString = queryString.Add("Input2", Input2.ToString());
            }
            if (!string.IsNullOrWhiteSpace(Input3))
            {
                queryString = queryString.Add("Input3", Input3.ToString());
            }
            using var result = await _client.GetAsync($"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}?Action=UrlWithParametersFromRoute2&Input3=%7BInput3%7D{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string UrlWithParametersFromRoute2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Input1", Input1.ToString());
            queryString = queryString.Add("Input2", Input2.ToString());
            queryString = queryString.Add("Input3", Input3.ToString());
            return $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}?Action=UrlWithParametersFromRoute2&Input3=%7BInput3%7D{queryString}";
        }

        public async Task<Response<byte[]>> GetBytes(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast?Action=GetBytes{queryString}", cancellationToken: _token);
            return new Response<byte[]>(
                result.StatusCode,
                result.Headers,
                (result.Content?.ReadAsByteArrayAsync()
                        ?? Task.FromResult<byte[]?>(default)));
        }
        public string GetBytes_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast?Action=GetBytes{queryString}";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingle(int Id , QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast?Action=GetSingle&Id=%7BId%7D{queryString}", cancellationToken: _token);
            if (_typeInfo != default)
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token , jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
            else
            {
                return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)));
            }
        }
        public string GetSingle_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("Id", Id.ToString());
            return $"/WeatherForecast?Action=GetSingle&Id=%7BId%7D{queryString}";
        }

        public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingleFromServiceExample(QueryString queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            using var result = await _client.GetAsync($"/WeatherForecast?Action=GetSingleFromServiceExample{queryString}", cancellationToken: _token);
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
            return $"/WeatherForecast?Action=GetSingleFromServiceExample{queryString}";
        }

        public async Task<Response> FormUploadTest1(System.Net.Http.MultipartFormDataContent multiPartContent, QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsync($"/WeatherForecast?Action=FormUploadTest1&formFile=%7BformFile%7D{queryString}", multiPartContent, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string FormUploadTest1_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("formFile", formFile.ToString());
            return $"/WeatherForecast?Action=FormUploadTest1&formFile=%7BformFile%7D{queryString}";
        }

        public async Task<Response> FormUploadTest2(System.Net.Http.MultipartFormDataContent? multiPartContent, QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.PostAsync($"/WeatherForecast?Action=FormUploadTest2&formFile=%7BformFile%7D{queryString}", multiPartContent, cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string FormUploadTest2_Url (QueryString queryString = default)
        {
            queryString = queryString.Add("formFile", formFile.ToString());
            return $"/WeatherForecast?Action=FormUploadTest2&formFile=%7BformFile%7D{queryString}";
        }

        public async Task<Response> TaskIssue(QueryString queryString = default, CancellationToken _token = default )
        {
            using var result = await _client.GetAsync($"/WeatherForecast?Action=TaskIssue{queryString}", cancellationToken: _token);
            return new Response(result.StatusCode, result.Headers);
        }
        public string TaskIssue_Url (QueryString queryString = default)
        {
            return $"/WeatherForecast?Action=TaskIssue{queryString}";
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
*/
