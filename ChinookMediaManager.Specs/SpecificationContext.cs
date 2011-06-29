using ChinookMediaManager.Core.Persistence;
using ChinookMediaManager.Domain.Configuration;
using FluentNHibernate.Cfg.Db;
using Machine.Specifications;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ChinookMediaManager.Specs
{
    public class SpecificationContext : NHibernateConfiguration, IAssemblyContext, ICleanupAfterEveryContextInAssembly
    {
        public static ISession Session;

        public SpecificationContext() : base(new []{ new EntityAutoPersistenceModelConfiguration()})
        {
        }

        protected override System.Func<IPersistenceConfigurer> GetSqlConfiguration()
        {
            return () => SQLiteConfiguration.Standard.InMemory().ShowSql();
        }

        protected void RebuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaExport(config).Execute(true, true,false, Session.Connection, null);
        }

        public void OnAssemblyStart()
        {
            var sessionFactory = CreateSessionFactory(GetSqlConfiguration(), BuildSchema);
            Session = sessionFactory.OpenSession(new DataBindingIntercepter(sessionFactory));
            RebuildSchema(SchemaConfig);
        }

        public void OnAssemblyComplete()
        {
            Session.Dispose();
        }

        public void AfterContextCleanup()
        {
            var sessionFactory = CreateSessionFactory();
            Session = SessionFactory.OpenSession(new DataBindingIntercepter(sessionFactory));
            RebuildSchema(SchemaConfig);
        }
    }

    // http://www.tigraine.at/2009/05/29/fluent-nhibernate-gotchas-when-testing-with-an-in-memory-database/
}