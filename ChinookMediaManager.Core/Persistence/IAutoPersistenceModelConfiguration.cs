using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Core.Persistence
{
    public interface IAutoPersistenceModelConfiguration
    {
        AutoPersistenceModel GetModel();
    }
}