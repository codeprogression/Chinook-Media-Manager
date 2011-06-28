using ChinookMediaManager.Core.Persistence;
using ChinookMediaManager.Domain;
using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Configuration
{
    public class EntityAutoPersistenceModelConfiguration : IAutoPersistenceModelConfiguration
    {
        public AutoPersistenceModel GetModel()
        {
            return AutoMap
                .AssemblyOf<Album>(new EntityAutoMappingConfiguration())
                .IgnoreBase<Entity>()
                .UseOverridesFromAssemblyOf<Album>()
                .Conventions.AddFromAssemblyOf<Album>()
                ;
        }
    }
}