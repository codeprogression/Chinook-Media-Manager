using FluentNHibernate.Cfg;

namespace ChinookMediaManager.Infrastructure.Persistence
{
    public interface IHbmMappingConfiguration
    {
        HbmMappingsContainer Configure(HbmMappingsContainer hbmMappings);
    }
}