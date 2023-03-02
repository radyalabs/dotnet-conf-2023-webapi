using System.Security.Claims;
using dotnetConf2023.Shared.Abstraction.Contexts;

namespace dotnetConf2023.Shared.Infrastructure.Contexts;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Username { get; } = default!;
    public Dictionary<string, IEnumerable<string>> Claims { get; }
    public List<string> Roles { get; }

    private IdentityContext()
    {
        Roles = new List<string>();
        Claims = new Dictionary<string, IEnumerable<string>>();
    }

    public IdentityContext(Guid? id)
    {
        Id = id ?? Guid.Empty;
        IsAuthenticated = id.HasValue;

        Roles = new List<string>();
        Claims = new Dictionary<string, IEnumerable<string>>();
    }

    public IdentityContext(ClaimsPrincipal principal)
    {
        Roles = new List<string>();
        Claims = new Dictionary<string, IEnumerable<string>>();

        if (principal.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
            return;

        IsAuthenticated = principal.Identity?.IsAuthenticated is true;

        Id = IsAuthenticated ? Guid.Parse(principal.Identity!.Name) : Guid.Empty;

        if (principal.Claims.Any(e => e.Type == ClaimTypes.Role))
            foreach (var claim in principal.Claims.Where(e => e.Type == ClaimTypes.Role))
                Roles.Add(claim.Value);

        Username = principal.Claims.SingleOrDefault(x => x.Type == "usr")?.Value!;
        Claims = principal.Claims.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Select(c => c.Value.ToString()));
    }

    public static IIdentityContext Empty => new IdentityContext();
}