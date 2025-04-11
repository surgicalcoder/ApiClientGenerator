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

namespace GoLive.Generator.ApiClientGenerator.URLs;
public class GeneratedURLs
{
    public static string InheritingTwo_GetPagedApiTest(int Page = 1, string Filter = null, int PageSize = 20, QueryString queryString = default)
    {
        queryString = queryString.Add("Filter", Filter.ToString());
        return $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}";
    }

    public static string InheritingTwo_GetApiTest2(int Page = 1, QueryString queryString = default)
    {
        return $"/ThisIsTestTwo/{Page}{queryString}";
    }

    public static string InheritingTwo_Get(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser2(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_OverrideTest(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser4(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingUser2_Get(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser2(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_OverrideTest(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser4(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string NonApi_TestModelBinderDifferentNameUnderId(System.String Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}";
    }

    public static string NonApi_TestModelBinderDifferentNameUnderId3(System.String Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}";
    }

    public static string TestIssue_Get(string Id = "", QueryString queryString = default)
    {
        return $"/api/TestIssue/Get/{Id}{queryString}";
    }

    public static string User_Get(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public static string User_GetUser(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/User{queryString}";
    }

    public static string User_GetUser(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public static string User_GetUser2(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/User{queryString}";
    }

    public static string User_OverrideTest(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/User{queryString}";
    }

    public static string User_GetUser4(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/User{queryString}";
    }

    public static string WeatherForecast_TestIgnoreGenericParmaeter(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestIgnoreNormalParameter(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestIgnoreWithCustomAttribute(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestModelBinderDifferentName(System.String OtherName, QueryString queryString = default)
    {
        queryString = queryString.Add("OtherName", OtherName.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestRemapTypeWithAnotherType(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestRemapTypeWithAnotherType2(System.String option, QueryString queryString = default)
    {
        queryString = queryString.Add("option", option.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_Get(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_UrlWithParametersFromRoute(string Input1, string Input2, QueryString queryString = default)
    {
        return $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}";
    }

    public static string WeatherForecast_UrlWithParametersFromRoute2(string Input1, string Input2, string Input3, QueryString queryString = default)
    {
        queryString = queryString.Add("Input3", Input3.ToString());
        return $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}";
    }

    public static string WeatherForecast_GetBytes(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_GetSingle(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_GetSingleFromServiceExample(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_FormUploadTest1(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_FormUploadTest2(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TaskIssue(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string YetAnother_YetAnotherTest(string Id, QueryString queryString = default)
    {
        return $"/api/YetAnother/YetAnotherTest/{Id}{queryString}";
    }

    public static string YetAnother_JSONDynamicTestDynamic(QueryString queryString = default)
    {
        return $"/api/YetAnother/JSONDynamicTestDynamic{queryString}";
    }

    public static string YetAnother_HttpOptionsTest(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpOptionsTest{queryString}";
    }

    public static string YetAnother_HttpHeadTest(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpHeadTest{queryString}";
    }

    public static string YetAnother_HttpPatchTest(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpPatchTest{queryString}";
    }
}