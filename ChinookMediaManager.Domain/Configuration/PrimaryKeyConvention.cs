using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace ChinookMediaManager.Domain.Configuration
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name+"Id");
        }

    }
}