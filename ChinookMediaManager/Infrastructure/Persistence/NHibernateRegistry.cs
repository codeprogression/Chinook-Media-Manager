using NHibernate;

namespace ChinookMediaManager.Infrastructure.Persistence
{
    public class NHibernateRegistry : StructureMap.Configuration.DSL.Registry
    {
        public NHibernateRegistry()
        {
            Scan(s =>
            {
                s.AssembliesFromApplicationBaseDirectory();
                s.AddAllTypesOf<IAutoPersistenceModelConfiguration>();
                s.AddAllTypesOf<IFluentMappingConfiguration>();
                s.AddAllTypesOf<IHbmMappingConfiguration>();
            });

            ForSingletonOf<ISessionFactory>().Use(c => c.GetInstance<NHibernateConfiguration>().CreateSessionFactory());

            // TODO: Implement Session Per Conversation
            For<ISession>().Use(c =>
            {
                var sessionFactory = c.GetInstance<ISessionFactory>();
                var session = sessionFactory.OpenSession(c.GetInstance<DataBindingIntercepter>());
                session.BeginTransaction();
                return session;
            });

            For<IStatelessSession>().Use(c => c.GetInstance<ISessionFactory>().OpenStatelessSession());
        }
    }
}