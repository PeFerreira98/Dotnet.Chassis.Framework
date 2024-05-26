using Serilog.Core;
using Serilog.Events;

namespace PeFerreira98.Chassis.Framework.Logging.LogEnricher;

internal class TraceContextLogEventEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) =>
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", System.Diagnostics.Activity.Current?.Id));
}
