namespace Perf360.Server.Services;

public class CurrentUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Unable to get the http context.");
        IsAuthenticated = httpContext.User.Identity!.IsAuthenticated;
        Name = httpContext.User.Identity!.Name;
        Id = httpContext.User.Claims.First(c => c.Type == "uid").Value;
    }

    public bool IsAuthenticated { get; }

    public string? Id { get; }

    public string? Name { get; }
}
