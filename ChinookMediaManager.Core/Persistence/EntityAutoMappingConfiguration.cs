using System;
using System.Linq;
using FluentNHibernate.Automapping;

namespace ChinookMediaManager.Core.Persistence
{
    public class EntityAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IPersistable));
        }
    }
}