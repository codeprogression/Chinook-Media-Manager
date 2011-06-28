using System;
using ChinookMediaManager.Core.Persistence;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Data;

namespace ChinookMediaManager.Configuration
{
    public class EntityAutoPersistenceModelConfiguration : IAutoPersistenceModelConfiguration
    {
        public AutoPersistenceModel GetModel()
        {
            return AutoMap
                .AssemblyOf<Entity>(new EntityAutoMappingConfiguration())
                .IgnoreBase<Entity>()
                .UseOverridesFromAssemblyOf<Entity>()
                .Conventions.AddFromAssemblyOf<Entity>()
                ;
        }
    }

    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name+"Id");
        }

    }
    public class ForeignKeyNamePlusIdConvention : ForeignKeyConvention
    {

        protected override string GetKeyName(Member property, Type type)
        {
            return property == null ? type.Name + "Id" : property.Name + "Id";
        }
    }
}