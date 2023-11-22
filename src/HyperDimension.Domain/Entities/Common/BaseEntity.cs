using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HyperDimension.Domain.Events.Common;

namespace HyperDimension.Domain.Entities.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid EntityId { get; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }

    private readonly List<BaseEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }
}
