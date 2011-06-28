using FluentNHibernate.Cfg;

namespace ChinookMediaManager.Core.Persistence
{
    public interface IFluentMappingConfiguration
    {
        FluentMappingsContainer Configure(FluentMappingsContainer fluentMappings);
    }
}