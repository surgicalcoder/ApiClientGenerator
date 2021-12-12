namespace GoLive.Generator.ApiClientGenerator
{
    public record ControllerRoute(string Name, string? Area, string BaseRoute, ActionRoute[] Actions);
}