using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Infrastructure.Persistence
{
    public interface IAutoPersistenceModelConfiguration
    {
        AutoPersistenceModel GetModel();
    }
}