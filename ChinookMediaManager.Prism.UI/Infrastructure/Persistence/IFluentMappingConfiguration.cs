using FluentNHibernate.Cfg;

namespace ChinookMediaManager.Infrastructure.Persistence
{
    public interface IFluentMappingConfiguration
    {
        FluentMappingsContainer Configure(FluentMappingsContainer fluentMappings);
    }
}