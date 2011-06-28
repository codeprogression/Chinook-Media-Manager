using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace ChinookMediaManager.Domain.Configuration
{
    public class ForeignKeyNamePlusIdConvention : ForeignKeyConvention
    {

        protected override string GetKeyName(Member property, Type type)
        {
            return property == null ? type.Name + "Id" : property.Name + "Id";
        }
    }
}