using dotnetConf2023.Shared.Abstraction.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetConf2023.Shared.Infrastructure.Auth;

public class RequirementManager
{
    private readonly IServiceProvider _serviceProvider;

    public RequirementManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Requirements = new Dictionary<string, string>();
    }

    public void Populate()
    {
        var list = _serviceProvider.GetServices<IRequirement>();
        var requirements = list.ToList();
        if (!requirements.Any()) return;

        foreach (var item in requirements)
        {
            if (Requirements.ContainsKey(item.Policy))
                throw new Exception($"Policy {item.Policy} already added");
            Requirements.Add(item.Policy, item.Permission);
        }
    }

    public Dictionary<string, string> Requirements { get; }
}