// ReSharper disable All
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
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

    public class InheritingTwoClient
    {
        private readonly HttpClient _client;

        public InheritingTwoClient (HttpClient client)
        {
            _client = client;
        }

        public async Task GetPagedApiTest(int Page  = 1, string Filter  = null, int PageSize  = 20, Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Filter) && !queryString.ContainsKey("Filter") )
            {
                queryString.Add("Filter", Filter.ToString());
            }
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/InheritingTwo/{Page}/{PageSize}", queryString), cancellationToken: _token);
        }
        public string GetPagedApiTest_Url (int Page  = 1,string Filter  = null,int PageSize  = 20, Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Filter) && !queryString.ContainsKey("Filter") )
            {
                queryString.Add("Filter", Filter.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/InheritingTwo/{Page}/{PageSize}", queryString), queryString);
        }

        public async Task GetApiTest2(int Page  = 1, Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/ThisIsTestTwo/{Page}", queryString), cancellationToken: _token);
        }
        public string GetApiTest2_Url (int Page  = 1, Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/ThisIsTestTwo/{Page}", queryString), queryString);
        }

        public async Task<global::System.Collections.Generic.IEnumerable<string>?> Get(Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token);
            }
        }
        public string Get_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), queryString);
        }

        public async Task<string?> GetUser(int Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (int Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), queryString);
        }

        public async Task<int> GetUser(string user , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            queryString ??= new();
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), queryString);
        }

        public async Task<string?> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), queryString);
        }

        public async Task<string?> OverrideTest(string Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string OverrideTest_Url (string Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingTwo/", queryString), queryString);
        }
    }

    public class InheritingUser2Client
    {
        private readonly HttpClient _client;

        public InheritingUser2Client (HttpClient client)
        {
            _client = client;
        }

        public async Task<global::System.Collections.Generic.IEnumerable<string>?> Get(Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token);
            }
        }
        public string Get_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), queryString);
        }

        public async Task<string?> GetUser(int Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (int Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), queryString);
        }

        public async Task<int> GetUser(string user , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            queryString ??= new();
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), queryString);
        }

        public async Task<string?> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), queryString);
        }

        public async Task<string?> OverrideTest(string Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string OverrideTest_Url (string Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/InheritingUser2/", queryString), queryString);
        }
    }

    public class NonApiClient
    {
        private readonly HttpClient _client;

        public NonApiClient (HttpClient client)
        {
            _client = client;
        }

        public async Task TestModelBinderDifferentNameUnderId(System.String Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}", queryString), cancellationToken: _token);
        }
        public string TestModelBinderDifferentNameUnderId_Url (System.String Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}", queryString), queryString);
        }
    }

    public class UserClient
    {
        private readonly HttpClient _client;

        public UserClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<global::System.Collections.Generic.IEnumerable<string>?> Get(Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<string>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<string>>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<string>>(cancellationToken: _token);
            }
        }
        public string Get_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), queryString);
        }

        public async Task<string?> GetUser(int Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (int Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), queryString);
        }

        public async Task<int> GetUser(string user , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<int> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(int)) as JsonTypeInfo<int>;
            }
            queryString ??= new();
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), user, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<int>(cancellationToken: _token);
            }
        }
        public string GetUser_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), queryString);
        }

        public async Task<string?> GetUser2(string Id , string Id2 , GoLive.Generator.ApiClientGenerator.Tests.WebApi.Controllers.UserController.ComplexObjectExample example , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), example, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string GetUser2_Url (string Id ,string Id2 , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Id2) && !queryString.ContainsKey("Id2") )
            {
                queryString.Add("Id2", Id2.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), queryString);
        }

        public async Task<string?> OverrideTest(string Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string OverrideTest_Url (string Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/User/", queryString), queryString);
        }
    }

    public class WeatherForecastClient
    {
        private readonly HttpClient _client;

        public WeatherForecastClient (HttpClient client)
        {
            _client = client;
        }

        public async Task TestIgnoreGenericParmaeter(string optionNotRemoved , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), new {}, cancellationToken: _token);
        }
        public string TestIgnoreGenericParmaeter_Url (string optionNotRemoved , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TestIgnoreNormalParameter(string optionNotRemoved , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), new {}, cancellationToken: _token);
        }
        public string TestIgnoreNormalParameter_Url (string optionNotRemoved , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TestIgnoreWithCustomAttribute(string optionNotRemoved , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), new {}, cancellationToken: _token);
        }
        public string TestIgnoreWithCustomAttribute_Url (string optionNotRemoved , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(optionNotRemoved) && !queryString.ContainsKey("optionNotRemoved") )
            {
                queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TestModelBinderDifferentName(System.String OtherName , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(OtherName) && !queryString.ContainsKey("OtherName") )
            {
                queryString.Add("OtherName", OtherName.ToString());
            }
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), new {}, cancellationToken: _token);
        }
        public string TestModelBinderDifferentName_Url (System.String OtherName , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(OtherName) && !queryString.ContainsKey("OtherName") )
            {
                queryString.Add("OtherName", OtherName.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TestRemapTypeWithAnotherType(System.TimeSpan option , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), option, cancellationToken: _token);
        }
        public string TestRemapTypeWithAnotherType_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TestRemapTypeWithAnotherType2(System.String option , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), option, cancellationToken: _token);
        }
        public string TestRemapTypeWithAnotherType2_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>?> Get(Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>)) as JsonTypeInfo<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>>(cancellationToken: _token);
            }
        }
        public string Get_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task SecretUrl(Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/_secretUrl", queryString), cancellationToken: _token);
        }
        public string SecretUrl_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/_secretUrl", queryString), queryString);
        }

        public async Task UrlWithParametersFromRoute(string Input1 , string Input2 , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}", queryString), cancellationToken: _token);
        }
        public string UrlWithParametersFromRoute_Url (string Input1 ,string Input2 , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}", queryString), queryString);
        }

        public async Task UrlWithParametersFromRoute2(string Input1 , string Input2 , string Input3 , Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Input3) && !queryString.ContainsKey("Input3") )
            {
                queryString.Add("Input3", Input3.ToString());
            }
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}", queryString), cancellationToken: _token);
        }
        public string UrlWithParametersFromRoute2_Url (string Input1 ,string Input2 ,string Input3 , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            if (!string.IsNullOrWhiteSpace(Input3) && !queryString.ContainsKey("Input3") )
            {
                queryString.Add("Input3", Input3.ToString());
            }
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}", queryString), queryString);
        }

        public async Task<byte[]?> GetBytes(Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), cancellationToken: _token);
            return await result.Content?.ReadAsByteArrayAsync();
        }
        public string GetBytes_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?> GetSingle(int Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token);
            }
        }
        public string GetSingle_Url (int Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast?> GetSingleFromServiceExample(Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast)) as JsonTypeInfo<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>;
            }
            queryString ??= new();
            using var result = await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>(cancellationToken: _token);
            }
        }
        public string GetSingleFromServiceExample_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task FormUploadTest1(System.Net.Http.MultipartFormDataContent multiPartContent, Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.PostAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), multiPartContent, cancellationToken: _token);
        }
        public string FormUploadTest1_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task FormUploadTest2(System.Net.Http.MultipartFormDataContent? multiPartContent, Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.PostAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), multiPartContent, cancellationToken: _token);
        }
        public string FormUploadTest2_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }

        public async Task TaskIssue(Dictionary<string, string?> queryString = default, CancellationToken _token = default )
        {
            queryString ??= new();
            await _client.GetAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), cancellationToken: _token);
        }
        public string TaskIssue_Url (Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/WeatherForecast/", queryString), queryString);
        }
    }

    public class YetAnotherClient
    {
        private readonly HttpClient _client;

        public YetAnotherClient (HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> YetAnotherTest(string Id , Dictionary<string, string?> queryString = default, CancellationToken _token = default , JsonTypeInfo<string> _typeInfo = default)
        {
            if (_typeInfo == default)
            {
                _typeInfo = ApiJsonSerializerContext.Default.GetTypeInfo(typeof(string)) as JsonTypeInfo<string>;
            }
            queryString ??= new();
            using var result = await _client.PostAsJsonAsync(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/YetAnother/YetAnotherTest/{Id}", queryString), new {}, cancellationToken: _token);
            if (_typeInfo != default)
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token , jsonTypeInfo: _typeInfo);
            }
            else
            {
                return await result.Content?.ReadFromJsonAsync<string>(cancellationToken: _token);
            }
        }
        public string YetAnotherTest_Url (string Id , Dictionary<string, string?> queryString = default)
        {
            queryString ??= new();
            return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString($"/api/YetAnother/YetAnotherTest/{Id}", queryString), queryString);
        }
    }
}
// JSON Source Generator
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<string>))]
[JsonSerializable(typeof(global::System.Collections.Generic.IEnumerable<global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast>))]
[JsonSerializable(typeof(global::GoLive.Generator.ApiClientGenerator.Tests.WebApi.WeatherForecast))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
// JSON Source Generator
// ReSharper disable All
