using System;
using System.Linq;
using ChinookMediaManager.Core.Persistence;
using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Configuration
{
    public class EntityAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IPersistable));
        }
    }
}