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

namespace GoLive.Generator.ApiClientGenerator.Tests.WebApi.Generated;
public class ApiClient
{
    public ApiClient(HttpClient client)
    {
        FormValueInGet = new FormValueInGetClient(client);
        InheritingButDifferentType = new InheritingButDifferentTypeClient(client);
        InheritingTwo = new InheritingTwoClient(client);
        InheritingUser2 = new InheritingUser2Client(client);
        NonApi = new NonApiClient(client);
        TestIssue = new TestIssueClient(client);
        User = new UserClient(client);
        WeatherForecast = new WeatherForecastClient(client);
        YetAnother = new YetAnotherClient(client);
    }

    public FormValueInGetClient FormValueInGet { get; }
    public InheritingButDifferentTypeClient InheritingButDifferentType { get; }
    public InheritingTwoClient InheritingTwo { get; }
    public InheritingUser2Client InheritingUser2 { get; }
    public NonApiClient NonApi { get; }
    public TestIssueClient TestIssue { get; }
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
    public HttpResponseHeaders Headers { get; set; }
    public DateTimeOffset? RequestStart { get; set; }
    public DateTimeOffset? RequestEnd { get; set; }
    public TimeSpan? RequestDuration => RequestStart.HasValue && RequestEnd.HasValue ? RequestEnd.Value - RequestStart.Value : null;

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
        if (Headers != null && Headers.Contains("X-Correlation-Id"))
        {
            CorrelationId = Headers.GetValues("X-Correlation-Id");
        }
    }

    public HttpStatusCode StatusCode { get; }
    public bool Success => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
    public IEnumerable<string> CorrelationId { get; set; } = [];
}

public class Response<T> : Response
{
    public Response(HttpResponseHeaders headers) : base(headers)
    {
    }

    public Response(HttpStatusCode statusCode, HttpResponseHeaders headers, Task<T?> data) : base(statusCode, headers)
    {
        Data = data;
        this.Headers = headers;
        if (headers != null)
        {
            _populateValuesFromHeaders();
        }
    }

    public Task<T?>? Data { get; }
    public Task<T?> SuccessData => Success ? Data ?? throw new EmptyBodyException((int)StatusCode, Headers) : throw new UnsuccessfulException((int)StatusCode, Headers);

    public bool TryGetSuccessData([NotNullWhen(true)] out Task<T?> data)
    {
        data = Data!;
        return Success && data is not null;
    }
}

public class FormValueInGetClient
{
    private readonly HttpClient _client;
    public FormValueInGetClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response> FormValueInQuerystringTest(GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.TestItem item, QueryString queryString = default, CancellationToken _token = default)
    {
        if (item != default)
        {
            queryString = queryString.Add("item", System.Text.Json.JsonSerializer.Serialize(item, ApiJsonSerializerContext.Default.Options));
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/FormValueInGet/FormValueInQuerystringTest{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string FormValueInQuerystringTest_Url(GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.TestItem item, QueryString queryString = default)
    {
        queryString = queryString.Add("item", System.Text.Json.JsonSerializer.Serialize(item, ApiJsonSerializerContext.Default.Options));
        return $"/api/FormValueInGet/FormValueInQuerystringTest{queryString}";
    }
}

public class InheritingButDifferentTypeClient
{
    private readonly HttpClient _client;
    public InheritingButDifferentTypeClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingButDifferentType{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string Get_Url(QueryString queryString = default)
    {
        return $"/InheritingButDifferentType{queryString}";
    }

    public async Task<Response<string>> GetUser(int Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id != default)
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingButDifferentType{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingButDifferentType{queryString}";
    }

    public async Task<Response<string>> GetUser4(int Id3, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id3 != default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingButDifferentType{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", System.Text.Json.JsonSerializer.Serialize(Id3, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingButDifferentType{queryString}";
    }

    public async Task<Response<int>> OverrideTest(string Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<int>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingButDifferentType/InheritingButDifferentType{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingButDifferentType/InheritingButDifferentType{queryString}";
    }

    public async Task<Response<int>> GetUser(string user, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<int>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingButDifferentType{queryString}");
        request.Content = JsonContent.Create(user);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(QueryString queryString = default)
    {
        return $"/InheritingButDifferentType{queryString}";
    }

    public async Task<Response<string>> GetUser2(string Id, string Id2, GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        if (!string.IsNullOrWhiteSpace(Id2))
        {
            queryString = queryString.Add("Id2", Id2.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingButDifferentType{queryString}");
        request.Content = JsonContent.Create(example);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        queryString = queryString.Add("Id2", System.Text.Json.JsonSerializer.Serialize(Id2, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingButDifferentType{queryString}";
    }
}

public class InheritingTwoClient
{
    private readonly HttpClient _client;
    public InheritingTwoClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingTwo{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string Get_Url(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public async Task<Response<string>> GetUser(int Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id != default)
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingTwo{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingTwo{queryString}";
    }

    public async Task<Response<string>> OverrideTest(string Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingTwo{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingTwo{queryString}";
    }

    public async Task<Response<string>> GetUser4(int Id3, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id3 != default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingTwo{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", System.Text.Json.JsonSerializer.Serialize(Id3, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingTwo{queryString}";
    }

    public async Task<Response> GetPagedApiTest(int Page = 1, string Filter = "", int PageSize = 20, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            queryString = queryString.Add("Filter", Filter.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string GetPagedApiTest_Url(int Page = 1, string Filter = "", int PageSize = 20, QueryString queryString = default)
    {
        queryString = queryString.Add("Filter", System.Text.Json.JsonSerializer.Serialize(Filter, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}";
    }

    public async Task<Response> GetApiTest2(int Page = 1, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/ThisIsTestTwo/{Page}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string GetApiTest2_Url(int Page = 1, QueryString queryString = default)
    {
        return $"/ThisIsTestTwo/{Page}{queryString}";
    }

    public async Task<Response<int>> GetUser(string user, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<int>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingTwo{queryString}");
        request.Content = JsonContent.Create(user);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public async Task<Response<string>> GetUser2(string Id, string Id2, GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        if (!string.IsNullOrWhiteSpace(Id2))
        {
            queryString = queryString.Add("Id2", Id2.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingTwo{queryString}");
        request.Content = JsonContent.Create(example);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        queryString = queryString.Add("Id2", System.Text.Json.JsonSerializer.Serialize(Id2, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingTwo{queryString}";
    }
}

public class InheritingUser2Client
{
    private readonly HttpClient _client;
    public InheritingUser2Client(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingUser2{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string Get_Url(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public async Task<Response<string>> GetUser(int Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id != default)
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingUser2{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingUser2{queryString}";
    }

    public async Task<Response<string>> OverrideTest(string Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingUser2{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingUser2{queryString}";
    }

    public async Task<Response<string>> GetUser4(int Id3, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id3 != default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/InheritingUser2{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", System.Text.Json.JsonSerializer.Serialize(Id3, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingUser2{queryString}";
    }

    public async Task<Response<int>> GetUser(string user, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<int>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingUser2{queryString}");
        request.Content = JsonContent.Create(user);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public async Task<Response<string>> GetUser2(string Id, string Id2, GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        if (!string.IsNullOrWhiteSpace(Id2))
        {
            queryString = queryString.Add("Id2", Id2.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/InheritingUser2{queryString}");
        request.Content = JsonContent.Create(example);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        queryString = queryString.Add("Id2", System.Text.Json.JsonSerializer.Serialize(Id2, ApiJsonSerializerContext.Default.Options));
        return $"/InheritingUser2{queryString}";
    }
}

public class NonApiClient
{
    private readonly HttpClient _client;
    public NonApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response> TestModelBinderDifferentNameUnderId(System.String Id, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestModelBinderDifferentNameUnderId_Url(string Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}";
    }

    public async Task<Response> TestModelBinderDifferentNameUnderId3(System.String Id, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestModelBinderDifferentNameUnderId3_Url(string Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}";
    }

    public readonly struct TestWithAllowedValues_DesiredState
    {
        private TestWithAllowedValues_DesiredState(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => $"{Value}";
        public static TestWithAllowedValues_DesiredState start = new TestWithAllowedValues_DesiredState("start");
        public static TestWithAllowedValues_DesiredState stop = new TestWithAllowedValues_DesiredState("stop");
        public static TestWithAllowedValues_DesiredState kill = new TestWithAllowedValues_DesiredState("kill");
        public static TestWithAllowedValues_DesiredState restart = new TestWithAllowedValues_DesiredState("restart");
    }

    public async Task<Response> TestWithAllowedValues(string Id, TestWithAllowedValues_DesiredState DesiredState = default, QueryString queryString = default, CancellationToken _token = default)
    {
        queryString = queryString.Add("DesiredState", DesiredState.Value.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/NonApi/TestWithAllowedValues/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestWithAllowedValues_Url(string Id, string DesiredState = "", QueryString queryString = default)
    {
        queryString = queryString.Add("DesiredState", System.Text.Json.JsonSerializer.Serialize(DesiredState, ApiJsonSerializerContext.Default.Options));
        return $"/api/NonApi/TestWithAllowedValues/{Id}{queryString}";
    }

    public readonly struct TestWithAllowedValuesButNullable_DesiredState
    {
        private TestWithAllowedValuesButNullable_DesiredState(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => $"{Value}";
        public static TestWithAllowedValuesButNullable_DesiredState start = new TestWithAllowedValuesButNullable_DesiredState("start");
        public static TestWithAllowedValuesButNullable_DesiredState stop = new TestWithAllowedValuesButNullable_DesiredState("stop");
        public static TestWithAllowedValuesButNullable_DesiredState kill = new TestWithAllowedValuesButNullable_DesiredState("kill");
        public static TestWithAllowedValuesButNullable_DesiredState restart = new TestWithAllowedValuesButNullable_DesiredState("restart");
    }

    public async Task<Response> TestWithAllowedValuesButNullable(string Id, TestWithAllowedValuesButNullable_DesiredState? DesiredState = default, QueryString queryString = default, CancellationToken _token = default)
    {
        if (DesiredState.HasValue)
        {
            queryString = queryString.Add("DesiredState", DesiredState.Value.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/NonApi/TestWithAllowedValuesButNullable/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestWithAllowedValuesButNullable_Url(string Id, string? DesiredState = default, QueryString queryString = default)
    {
        queryString = queryString.Add("DesiredState", System.Text.Json.JsonSerializer.Serialize(DesiredState, ApiJsonSerializerContext.Default.Options));
        return $"/api/NonApi/TestWithAllowedValuesButNullable/{Id}{queryString}";
    }

    public readonly struct TestWithAllowedValuesButNullableAndDefaultValue_DesiredState
    {
        private TestWithAllowedValuesButNullableAndDefaultValue_DesiredState(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => $"{Value}";
        public static TestWithAllowedValuesButNullableAndDefaultValue_DesiredState start = new TestWithAllowedValuesButNullableAndDefaultValue_DesiredState("start");
        public static TestWithAllowedValuesButNullableAndDefaultValue_DesiredState stop = new TestWithAllowedValuesButNullableAndDefaultValue_DesiredState("stop");
        public static TestWithAllowedValuesButNullableAndDefaultValue_DesiredState kill = new TestWithAllowedValuesButNullableAndDefaultValue_DesiredState("kill");
        public static TestWithAllowedValuesButNullableAndDefaultValue_DesiredState restart = new TestWithAllowedValuesButNullableAndDefaultValue_DesiredState("restart");
    }

    public async Task<Response> TestWithAllowedValuesButNullableAndDefaultValue(string Id, TestWithAllowedValuesButNullableAndDefaultValue_DesiredState? DesiredState = default, QueryString queryString = default, CancellationToken _token = default)
    {
        if (DesiredState.HasValue)
        {
            queryString = queryString.Add("DesiredState", DesiredState.Value.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/NonApi/TestWithAllowedValuesButNullableAndDefaultValue/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestWithAllowedValuesButNullableAndDefaultValue_Url(string Id, string DesiredState = "", QueryString queryString = default)
    {
        queryString = queryString.Add("DesiredState", System.Text.Json.JsonSerializer.Serialize(DesiredState, ApiJsonSerializerContext.Default.Options));
        return $"/api/NonApi/TestWithAllowedValuesButNullableAndDefaultValue/{Id}{queryString}";
    }
}

public class TestIssueClient
{
    private readonly HttpClient _client;
    public TestIssueClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response> Get(string Id = "", QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/TestIssue/Get/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string Get_Url(string Id = "", QueryString queryString = default)
    {
        return $"/api/TestIssue/Get/{Id}{queryString}";
    }
}

public class UserClient
{
    private readonly HttpClient _client;
    public UserClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::System.Collections.Generic.IEnumerable<string>>> Get(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/User{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::System.Collections.Generic.IEnumerable<string>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<string>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string Get_Url(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public async Task<Response<string>> GetUser(int Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id != default)
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/User{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/User{queryString}";
    }

    public async Task<Response<string>> OverrideTest(string Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/User{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/User{queryString}";
    }

    public async Task<Response<string>> GetUser4(int Id3, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (Id3 != default)
        {
            queryString = queryString.Add("Id3", Id3.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/User{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", System.Text.Json.JsonSerializer.Serialize(Id3, ApiJsonSerializerContext.Default.Options));
        return $"/User{queryString}";
    }

    public async Task<Response<int>> GetUser(string user, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<int>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/User{queryString}");
        request.Content = JsonContent.Create(user);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<int>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token) ?? Task.FromResult<int>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser_Url(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public async Task<Response<string>> GetUser2(string Id, string Id2, GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        if (!string.IsNullOrWhiteSpace(Id))
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        if (!string.IsNullOrWhiteSpace(Id2))
        {
            queryString = queryString.Add("Id2", Id2.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/User{queryString}");
        request.Content = JsonContent.Create(example);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        queryString = queryString.Add("Id2", System.Text.Json.JsonSerializer.Serialize(Id2, ApiJsonSerializerContext.Default.Options));
        return $"/User{queryString}";
    }
}

/// <summary>
///     This is a test of the XML Documentation feature for Weather Forecast Controller
///     </summary>
public class WeatherForecastClient
{
    private readonly HttpClient _client;
    public WeatherForecastClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>> Get(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token) ?? Task.FromResult<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string Get_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response<byte[]>> GetBytes(QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response<byte[]>(result.StatusCode, result.Headers, (result.Content?.ReadAsByteArrayAsync() ?? Task.FromResult<byte[]>(default)))
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string GetBytes_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingle(int Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
        }

        if (Id != default)
        {
            queryString = queryString.Add("Id", Id.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetSingle_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", System.Text.Json.JsonSerializer.Serialize(Id, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> GetSingleFromServiceExample(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string GetSingleFromServiceExample_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TaskIssue(QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TaskIssue_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    /// <summary>
    ///     This is a test of the XML Documentation feature for UrlWithParametersFromRoute
    ///     </summary><param name = "Input1">This is the string of the input param1</param><param name = "Input2">This is the string of the input param2</param><returns></returns>
    public async Task<Response> UrlWithParametersFromRoute(string Input1, string Input2, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string UrlWithParametersFromRoute_Url(string Input1, string Input2, QueryString queryString = default)
    {
        return $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}";
    }

    public async Task<Response> UrlWithParametersFromRoute2(string Input1, string Input2, string Input3, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(Input3))
        {
            queryString = queryString.Add("Input3", Input3.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string UrlWithParametersFromRoute2_Url(string Input1, string Input2, string Input3, QueryString queryString = default)
    {
        queryString = queryString.Add("Input3", System.Text.Json.JsonSerializer.Serialize(Input3, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}";
    }

    public async Task<Response> TestIgnoreGenericParmaeter(string optionNotRemoved, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(optionNotRemoved))
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestIgnoreGenericParmaeter_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", System.Text.Json.JsonSerializer.Serialize(optionNotRemoved, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TestIgnoreNormalParameter(string optionNotRemoved, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(optionNotRemoved))
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestIgnoreNormalParameter_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", System.Text.Json.JsonSerializer.Serialize(optionNotRemoved, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TestIgnoreWithCustomAttribute(string optionNotRemoved, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(optionNotRemoved))
        {
            queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestIgnoreWithCustomAttribute_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", System.Text.Json.JsonSerializer.Serialize(optionNotRemoved, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TestModelBinderDifferentName(System.String OtherName, QueryString queryString = default, CancellationToken _token = default)
    {
        if (!string.IsNullOrWhiteSpace(OtherName))
        {
            queryString = queryString.Add("OtherName", OtherName.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestModelBinderDifferentName_Url(string OtherName, QueryString queryString = default)
    {
        queryString = queryString.Add("OtherName", System.Text.Json.JsonSerializer.Serialize(OtherName, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TestRemapTypeWithAnotherType(System.TimeSpan option, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        request.Content = JsonContent.Create(option);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestRemapTypeWithAnotherType_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> TestRemapTypeWithAnotherType2(System.String option, QueryString queryString = default, CancellationToken _token = default)
    {
        if (option != default)
        {
            queryString = queryString.Add("option", option.ToString());
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string TestRemapTypeWithAnotherType2_Url(string option, QueryString queryString = default)
    {
        queryString = queryString.Add("option", System.Text.Json.JsonSerializer.Serialize(option, ApiJsonSerializerContext.Default.Options));
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> FormUploadTest1(System.Net.Http.MultipartFormDataContent multiPartContent, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        request.Content = multiPartContent;
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string FormUploadTest1_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public async Task<Response> FormUploadTest2(Microsoft.AspNetCore.Http.IFormFile? formFile, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/WeatherForecast{queryString}");
        request.Content = JsonContent.Create(formFile);
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string FormUploadTest2_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }
}

public class YetAnotherClient
{
    private readonly HttpClient _client;
    public YetAnotherClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>> JSONDynamicTestDynamic(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/YetAnother/JSONDynamicTestDynamic{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest>(cancellationToken: _token) ?? Task.FromResult<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string JSONDynamicTestDynamic_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/JSONDynamicTestDynamic{queryString}";
    }

    public async Task<Response> FromServiceTest(string Id, QueryString queryString = default, CancellationToken _token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/YetAnother/FromServiceTest/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        return new Response(result.StatusCode, result.Headers)
        {
            RequestStart = requestStarted,
            RequestEnd = requestEnd
        };
    }

    public string FromServiceTest_Url(string Id, QueryString queryString = default)
    {
        return $"/api/YetAnother/FromServiceTest/{Id}{queryString}";
    }

    public async Task<Response<string>> HttpHeadTest(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Head, $"/api/YetAnother/HttpHeadTest{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string HttpHeadTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpHeadTest{queryString}";
    }

    public async Task<Response<string>> HttpOptionsTest(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Options, $"/api/YetAnother/HttpOptionsTest{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string HttpOptionsTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpOptionsTest{queryString}";
    }

    public async Task<Response<string>> HttpPatchTest(QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        using var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/YetAnother/HttpPatchTest{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string HttpPatchTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpPatchTest{queryString}";
    }

    public async Task<Response<string>> YetAnotherTest(string Id, QueryString queryString = default, CancellationToken _token = default, JsonTypeInfo<string>? _typeInfo = default)
    {
        if (_typeInfo == default)
        {
            _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/YetAnother/YetAnotherTest/{Id}{queryString}");
        var requestStarted = DateTime.UtcNow;
        using var result = await _client.SendAsync(request, _token);
        var requestEnd = DateTime.UtcNow;
        if (_typeInfo != default)
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token, jsonTypeInfo: _typeInfo) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
        else
        {
            return new Response<string>(result.StatusCode, result.Headers, (result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token) ?? Task.FromResult<string?>(default)))
            {
                RequestStart = requestStarted,
                RequestEnd = requestEnd
            };
        }
    }

    public string YetAnotherTest_Url(string Id, QueryString queryString = default)
    {
        return $"/api/YetAnother/YetAnotherTest/{Id}{queryString}";
    }
}

// JSON Source Generator
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, AllowTrailingCommas = true)]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<string>))]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>))]
[JsonSerializable(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast))]
[JsonSerializable(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.JSONDynamicTest))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}// JSON Source Generator
// ReSharper disable All
