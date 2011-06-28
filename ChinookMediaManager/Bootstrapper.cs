using System.Windows;
using ChinookMediaManager.Core.Bootstrap;
using ChinookMediaManager.Core.Persistence;
using Microsoft.Practices.Prism.Modularity;

namespace ChinookMediaManager
{
    public class Bootstrapper: StructureMapBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shellView = Container.GetInstance<ShellView>();
            shellView.Show();
            return shellView;
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            var moduleCatalog = new ModuleCatalog();
            foreach (var module in Container.GetAllInstances<IModule>())
            {
                moduleCatalog.AddModule(module.GetType());
            }
            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
            Container.Configure(x =>
            {
                x.AddRegistry<NHibernateRegistry>();
                x.AddRegistry<ModuleRegistry>();
                x.Scan(s=>
                    {
                        s.AssembliesFromApplicationBaseDirectory();
                        s.WithDefaultConventions();
                    });
            });
            base.ConfigureContainer();

        }
    }
}