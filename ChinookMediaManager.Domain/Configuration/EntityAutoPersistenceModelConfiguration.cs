using ChinookMediaManager.Core.Persistence;
using ChinookMediaManager.Domain.Entities;
using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Domain.Configuration
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