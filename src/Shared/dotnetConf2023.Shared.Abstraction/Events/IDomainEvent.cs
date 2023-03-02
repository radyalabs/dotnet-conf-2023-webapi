using Mediator;

namespace dotnetConf2023.Shared.Abstraction.Events;

/// <summary>
/// Represents the interface for an event that is raised within the domain.
/// </summary>
public interface IDomainEvent : INotification
{
}