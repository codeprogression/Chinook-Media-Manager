using FluentNHibernate.Cfg;

namespace ChinookMediaManager.Core.Persistence
{
    public interface IHbmMappingConfiguration
    {
        HbmMappingsContainer Configure(HbmMappingsContainer hbmMappings);
    }
}