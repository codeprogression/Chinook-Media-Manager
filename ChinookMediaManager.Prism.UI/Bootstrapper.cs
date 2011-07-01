using System.Collections.Generic;
using System.Windows;
using ChinookMediaManager.Core.Persistence;
using ChinookMediaManager.Prism.UI.Bootstrap;
using Microsoft.Practices.Prism.Modularity;

namespace ChinookMediaManager.Prism.UI
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
            var modules = Container.GetAllInstances<IModule>();
            foreach (var module in modules)
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