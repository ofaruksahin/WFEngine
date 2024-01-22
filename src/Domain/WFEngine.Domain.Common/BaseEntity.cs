﻿using System.ComponentModel.DataAnnotations.Schema;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
        public int StatusId { get; set; }

        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();
        public EnumStatus Status
        {
            get => (EnumStatus)StatusId;
            set => StatusId = (int)value;
        }

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
