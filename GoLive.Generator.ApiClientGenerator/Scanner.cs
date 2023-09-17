using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GoLive.Generator.ApiClientGenerator
{
    public static class Scanner
    {
        private const string ROUTE_ATTR_NAME = "Microsoft.AspNetCore.Mvc.RouteAttribute";
        private const string ASPNETCORE_NAMESPACE = "Microsoft.AspNetCore.Mvc";
        private static SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public static bool CanBeController(SyntaxNode node)
            => node is ClassDeclarationSyntax c
               // Don't generate routes for abstract controllers
            && !c.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword))
            && c.BaseList?.Types.Count > 0;

        public static IEnumerable<ControllerRoute> ScanForControllers(SemanticModel semantic)
        {
            var allNodes = semantic.SyntaxTree.GetRoot().DescendantNodes();

            foreach (var node in allNodes) {
                if (CanBeController(node)
                 && semantic.GetDeclaredSymbol(node) is INamedTypeSymbol classSymbol
                 && IsController(classSymbol))
                    yield return ConvertToRoute(classSymbol);
            }
        }

        public static ControllerRoute ConvertToRoute(INamedTypeSymbol classSymbol)
        {
            const string suffix = "Controller";
            var name = classSymbol.Name.EndsWith(suffix)
                ? classSymbol.Name.Substring(0, classSymbol.Name.Length - suffix.Length)
                : classSymbol.Name;

            var actionMethods = ScanForActionMethods(classSymbol).ToList();

            var parentClass = classSymbol.BaseType;

            if (parentClass != null && !parentClass.ToDisplayString(symbolDisplayFormat).StartsWith(ASPNETCORE_NAMESPACE, StringComparison.InvariantCultureIgnoreCase))
            {
                var addRoutes = ConvertToRoute(parentClass);

                if (addRoutes != null && addRoutes.Actions.Any())
                {
                    actionMethods.AddRange(addRoutes.Actions);
                }
            }
            
            // Extract the route from the HttpActionAttribute
            var attribute = FindAttribute(classSymbol, a => a.ToString() == ROUTE_ATTR_NAME);
            var route = attribute?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;

            var areaAttribute = FindAttribute(classSymbol, a => a.ToString() == "Microsoft.AspNetCore.Mvc.AreaAttribute");
            var area = areaAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? null;
            return new ControllerRoute(name, area, route, actionMethods.ToArray());
        }

        private static IEnumerable<ActionRoute> ScanForActionMethods(INamedTypeSymbol classSymbol)
        {
            foreach (var member in classSymbol.GetMembers())
            {
                if (member
                    is IMethodSymbol { DeclaredAccessibility: Accessibility.Public, IsAbstract: false, IsOverride: false } methodSymbol // If it's overriden then the signature will be the same, so can filter this out here
                    and not { MethodKind: MethodKind.Constructor }
                   )
                {
                    if (methodSymbol.Name.StartsWith("get_") || methodSymbol.Name.StartsWith("set_"))
                    {
                        continue;
                    }

                    var name = methodSymbol.Name;
                    var returnType = methodSymbol.ReturnType;

                    // Unwrap Task<T>
                    if (returnType is INamedTypeSymbol taskType && taskType.OriginalDefinition.ToString() == "System.Threading.Tasks.Task<TResult>")
                    {
                        returnType = taskType.TypeArguments.First();
                    }

                    // Take unwrapped T and check whether we need to 
                    // unwrap further to V when T = ActionResult<V>
                    if (returnType is INamedTypeSymbol actionResultType && actionResultType.OriginalDefinition.ToString() == "Microsoft.AspNetCore.Mvc.ActionResult<TValue>")
                    {
                        returnType = actionResultType.TypeArguments.First();
                    }

                    // If the return type is simple IActionResult -- assume that the return type is essentially void
                    if (returnType.OriginalDefinition.ToString() == "Microsoft.AspNetCore.Mvc.IActionResult" || returnType.OriginalDefinition.ToString() == "Microsoft.AspNetCore.Mvc.ActionResult")
                    {
                        returnType = null;
                    }

                    // Extract the route from the HttpActionAttribute
                    var attribute = FindAttribute(methodSymbol, a => a.BaseType?.ToString() == "Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute");
                    var route = attribute?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;
                    var method = attribute?.AttributeClass?.Name switch
                    {
                        "HttpGetAttribute" => HttpMethod.Get,
                        "HttpPutAttribute" => HttpMethod.Put,
                        "HttpPostAttribute" => HttpMethod.Post,
                        "HttpDeleteAttribute" => HttpMethod.Delete,
                        _ => HttpMethod.Get
                        //   _ => throw new InvalidOperationException($"Unknown attribute {attribute?.AttributeClass?.Name}")
                    };

                    string parentRoutes = getParentRouteAttrs(classSymbol);
                    
                    var routeAttr = FindAttribute(methodSymbol, a => a.OriginalDefinition.ToString() == ROUTE_ATTR_NAME);

                    if (routeAttr != null)
                    {
                        route = routeAttr.ConstructorArguments.FirstOrDefault().Value.ToString() ?? string.Empty;
                    }

                    if (!string.IsNullOrWhiteSpace(parentRoutes) && !route.StartsWith("/"))
                    {
                        if (parentRoutes.EndsWith("/"))
                        {
                            parentRoutes = parentRoutes.Substring(0, parentRoutes.Length - 1);
                        }
                        
                        route = $"{parentRoutes}/{route}";
                    }

                    if (!string.IsNullOrWhiteSpace(route) && !route.StartsWith("/"))
                    {
                        route = $"/{route}";
                    }
                    
                    var customFormatterAttribute = FindAttribute(methodSymbol, a => a.Name == "FormFormatterAttribute");

                    bool useCustomFormatter = customFormatterAttribute != null;
                    
                    var parameters = methodSymbol.Parameters.Where(t => true)
                        .Select(delegate(IParameterSymbol t) { return new ParameterMapping(t.Name, new Parameter(t.Type.ToString(), t.HasExplicitDefaultValue, t.HasExplicitDefaultValue ? t.ExplicitDefaultValue : null)); })
                        .ToArray();
                    var bodyParameter = methodSymbol.Parameters.Where(t => (!IsPrimitive(t.Type)) || t.GetAttributes().Any(e => e.AttributeClass?.Name == "FromBodyAttribute"))
                        .Select(t => new ParameterMapping(t.Name, new Parameter(t.Type.ToString(), t.HasExplicitDefaultValue, t.HasExplicitDefaultValue ? t.ExplicitDefaultValue : null)))
                        .FirstOrDefault();

                    yield return new ActionRoute(name, method, route,
                        returnType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), returnType?.IsReferenceType != true,
                        useCustomFormatter, parameters, bodyParameter);
                }
            }
        }

        private static string getParentRouteAttrs(INamedTypeSymbol classSymbol, string route = "")
        {
            var classRouteAttr = FindAttribute(classSymbol, a => a.OriginalDefinition.ToString() == ROUTE_ATTR_NAME);

            if (classRouteAttr != null)
            {
                var routeVal = classRouteAttr.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;

                if (routeVal.StartsWith("/"))
                {
                    return routeVal;
                }
                
                route = $"{routeVal}/{route}";
            }
            
            if (classSymbol.BaseType != null && !classSymbol.BaseType.ToDisplayString(symbolDisplayFormat).StartsWith(ASPNETCORE_NAMESPACE, StringComparison.InvariantCultureIgnoreCase))
            {
                route = getParentRouteAttrs(classSymbol.BaseType, route);
            }

            return route;
        }

        public static bool IsController(INamedTypeSymbol classDeclaration)
            => InheritsFrom(classDeclaration, "Microsoft.AspNetCore.Mvc.ControllerBase");

        private static bool InheritsFrom(INamedTypeSymbol classDeclaration, string qualifiedBaseTypeName)
        {
            var currentDeclared = classDeclaration;

            while (currentDeclared.BaseType != null)
            {
                var currentBaseType = currentDeclared.BaseType;
                if (string.Equals(currentBaseType.ToDisplayString(symbolDisplayFormat), qualifiedBaseTypeName, StringComparison.Ordinal))
                {
                    return true;
                }

                currentDeclared = currentBaseType;
            }

            return false;
        }

        private static bool IsPrimitive(ITypeSymbol typeSymbol)
        {
            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_SByte:
                case SpecialType.System_Int16:
                case SpecialType.System_Int32:
                case SpecialType.System_Int64:
                case SpecialType.System_Byte:
                case SpecialType.System_UInt16:
                case SpecialType.System_UInt32:
                case SpecialType.System_UInt64:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_Char:
                case SpecialType.System_String:
                    return true;
            }

            switch (typeSymbol.TypeKind)
            {
                case TypeKind.Enum:
                    return true;
            }

            return false;
        }


        private static AttributeData? FindAttribute(ISymbol symbol, Func<INamedTypeSymbol, bool> selectAttribute)
            => symbol
                .GetAttributes()
                .LastOrDefault(a => a?.AttributeClass != null && selectAttribute(a.AttributeClass));
    }
}