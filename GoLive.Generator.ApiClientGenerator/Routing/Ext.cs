using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GoLive.Generator.ApiClientGenerator.Routing;

public static class ExArgumentNullExceptionExt
{
    public static void ThrowIfNull(object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) =>
        throw new ArgumentNullException(paramName);
}