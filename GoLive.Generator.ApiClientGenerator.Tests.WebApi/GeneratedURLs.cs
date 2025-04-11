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
    public static string InheritingTwo_GetPagedApiTest_Url(int Page = 1, string Filter = null, int PageSize = 20, QueryString queryString = default)
    {
        queryString = queryString.Add("Filter", Filter.ToString());
        return $"/InheritingTwo/InheritingTwo/{Page}/{PageSize}{queryString}";
    }

    public static string InheritingTwo_GetApiTest2_Url(int Page = 1, QueryString queryString = default)
    {
        return $"/ThisIsTestTwo/{Page}{queryString}";
    }

    public static string InheritingTwo_Get_Url(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser_Url(QueryString queryString = default)
    {
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingTwo_GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/InheritingTwo{queryString}";
    }

    public static string InheritingUser2_Get_Url(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser_Url(QueryString queryString = default)
    {
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string InheritingUser2_GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/InheritingUser2{queryString}";
    }

    public static string NonApi_TestModelBinderDifferentNameUnderId_Url(System.String Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId/{Id}{queryString}";
    }

    public static string NonApi_TestModelBinderDifferentNameUnderId3_Url(System.String Id, QueryString queryString = default)
    {
        return $"/api/NonApi/TestModelBinderDifferentNameUnderId3/{Id}{queryString}";
    }

    public static string TestIssue_Get_Url(string Id = "", QueryString queryString = default)
    {
        return $"/api/TestIssue/Get/{Id}{queryString}";
    }

    public static string User_Get_Url(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public static string User_GetUser_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/User{queryString}";
    }

    public static string User_GetUser_Url(QueryString queryString = default)
    {
        return $"/User{queryString}";
    }

    public static string User_GetUser2_Url(string Id, string Id2, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        queryString = queryString.Add("Id2", Id2.ToString());
        return $"/User{queryString}";
    }

    public static string User_OverrideTest_Url(string Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/User{queryString}";
    }

    public static string User_GetUser4_Url(int Id3, QueryString queryString = default)
    {
        queryString = queryString.Add("Id3", Id3.ToString());
        return $"/User{queryString}";
    }

    public static string WeatherForecast_TestIgnoreGenericParmaeter_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestIgnoreNormalParameter_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestIgnoreWithCustomAttribute_Url(string optionNotRemoved, QueryString queryString = default)
    {
        queryString = queryString.Add("optionNotRemoved", optionNotRemoved.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestModelBinderDifferentName_Url(System.String OtherName, QueryString queryString = default)
    {
        queryString = queryString.Add("OtherName", OtherName.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestRemapTypeWithAnotherType_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TestRemapTypeWithAnotherType2_Url(System.String option, QueryString queryString = default)
    {
        queryString = queryString.Add("option", option.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_Get_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_UrlWithParametersFromRoute_Url(string Input1, string Input2, QueryString queryString = default)
    {
        return $"/WeatherForecast/UrlWithParametersFromRoute/{Input1}/{Input2}{queryString}";
    }

    public static string WeatherForecast_UrlWithParametersFromRoute2_Url(string Input1, string Input2, string Input3, QueryString queryString = default)
    {
        queryString = queryString.Add("Input3", Input3.ToString());
        return $"/WeatherForecast/UrlWithParametersFromRoute2/{Input1}/{Input2}{queryString}";
    }

    public static string WeatherForecast_GetBytes_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_GetSingle_Url(int Id, QueryString queryString = default)
    {
        queryString = queryString.Add("Id", Id.ToString());
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_GetSingleFromServiceExample_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_FormUploadTest1_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_FormUploadTest2_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string WeatherForecast_TaskIssue_Url(QueryString queryString = default)
    {
        return $"/WeatherForecast{queryString}";
    }

    public static string YetAnother_YetAnotherTest_Url(string Id, QueryString queryString = default)
    {
        return $"/api/YetAnother/YetAnotherTest/{Id}{queryString}";
    }

    public static string YetAnother_JSONDynamicTestDynamic_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/JSONDynamicTestDynamic{queryString}";
    }

    public static string YetAnother_HttpOptionsTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpOptionsTest{queryString}";
    }

    public static string YetAnother_HttpHeadTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpHeadTest{queryString}";
    }

    public static string YetAnother_HttpPatchTest_Url(QueryString queryString = default)
    {
        return $"/api/YetAnother/HttpPatchTest{queryString}";
    }
}