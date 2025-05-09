﻿
using System;
using System.Reflection;

namespace ServiceDefault.Messaging.Events;

public record IntegrationEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType { get; set; } = Assembly.GetEntryAssembly()?.GetType().AssemblyQualifiedName!;
}
