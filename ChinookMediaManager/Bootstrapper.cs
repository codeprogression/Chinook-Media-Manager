using System;
using System.Collections.Generic;
using System.Windows;
using ChinookMediaManager.Infrastructure.Persistence;
using ChinookMediaManager.Views;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using NHibernate;

namespace ChinookMediaManager
{
    public class Bootstrapper: UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window) Shell;
            App.Current.MainWindow.Show();

        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            InitializeNHibernate();
        }
        private void InitializeNHibernate()
        {
            Func<IInterceptor> sessionLocalInterceptor = () => new DataBindingIntercepter { SessionFactory = Container.Resolve<ISessionFactory>() };

            Container.RegisterType<IAutoPersistenceModelConfiguration, EntityAutoPersistenceModelConfiguration>("EntityConfig",new ContainerControlledLifetimeManager());
            Container.RegisterCollection<IAutoPersistenceModelConfiguration>();
            Container.RegisterInstance(typeof (ISessionFactory), new NHibernateConfiguration().CreateSessionFactory());

            // TODO: Implement Session per Conversation
            Container.RegisterInstance(((ISessionFactory)Container.Resolve(typeof(ISessionFactory))).OpenSession(sessionLocalInterceptor.Invoke()));

        }
    }

    public static class UnityExtensions
    {
        public static void RegisterCollection<T>(this IUnityContainer container) where T : class
        {
            container.RegisterType<IEnumerable<T>>(new InjectionFactory(c => c.ResolveAll<T>()));
        }
    }
}