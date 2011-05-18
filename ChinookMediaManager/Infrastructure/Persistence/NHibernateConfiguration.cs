using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ChinookMediaManager.Infrastructure.Persistence
{
    public class NHibernateConfiguration
    {
        readonly IEnumerable<IAutoPersistenceModelConfiguration> _models;
        readonly IEnumerable<IFluentMappingConfiguration> _fluentMappings;
        readonly IEnumerable<IHbmMappingConfiguration> _hbmMappings;


        public NHibernateConfiguration() : this(null, null, null) { }
        public NHibernateConfiguration(IEnumerable<IAutoPersistenceModelConfiguration> models) : this(models, null, null) { }

        [InjectionConstructor]
        public NHibernateConfiguration(IEnumerable<IAutoPersistenceModelConfiguration> models, IEnumerable<IFluentMappingConfiguration> fluentMappings, IEnumerable<IHbmMappingConfiguration> hbmMappings)
        {
            _models = models ?? new List<IAutoPersistenceModelConfiguration>(new[]{ new EntityAutoPersistenceModelConfiguration()});
            _fluentMappings = fluentMappings ?? new List<IFluentMappingConfiguration>();
            _hbmMappings = hbmMappings ?? new List<IHbmMappingConfiguration>();
        }

        protected ISessionFactory SessionFactory;
        protected NHibernate.Cfg.Configuration SchemaConfig;

        public ISessionFactory CreateSessionFactory()
        {
            SessionFactory = CreateSessionFactory(GetSqlConfiguration(), BuildSchema);
            return SessionFactory;
        }

        protected virtual Func<IPersistenceConfigurer> GetSqlConfiguration()
        {
            var msSql2005 = MsSqlConfiguration.MsSql2008;
            Action<MsSqlConnectionStringBuilder> expression =
                c => c.FromConnectionStringWithKey(DatabaseConstants.ConnectionStringKey);
            return () => msSql2005.ConnectionString(expression)
                            .UseReflectionOptimizer()
                            .IsolationLevel(IsolationLevel.ReadUncommitted)
                            .ShowSql();
        }

        public virtual ISessionFactory CreateSessionFactory(Func<IPersistenceConfigurer> configuration,
                                                            Action<NHibernate.Cfg.Configuration> schema)
        {
            try
            {

                var configure = Fluently.Configure();
                var database = configure.Database(configuration);
                var mappings = database.Mappings(CreateMappings());
                var fluentConfiguration = mappings.ExposeConfiguration(c =>
                {
                    c.SetProperty("generate_statistics", "true");
                    SchemaConfig = c;
                });
                return fluentConfiguration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
//                Log.Fatal(this, "Failed to create session factory", ex);
                throw;
            }
        }

        protected virtual Action<MappingConfiguration> CreateMappings()
        {
            return m =>
            {
                foreach (var model in _models ?? new Collection<IAutoPersistenceModelConfiguration>())
                {
                    m.AutoMappings.Add(model.GetModel());
                }
                foreach (var mapping in _fluentMappings ?? new Collection<IFluentMappingConfiguration>())
                {
                    mapping.Configure(m.FluentMappings);
                }
                foreach (var mapping in _hbmMappings ?? new Collection<IHbmMappingConfiguration>())
                {
                    mapping.Configure(m.HbmMappings);
                }
            };
        }

        protected virtual void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }
    }
}