using System.Windows;
using ChinookMediaManager.Core.Persistence;
using ChinookMediaManager.Prism.UI.Bootstrap;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.Prism.UI
{
    public class Bootstrapper: StructureMapBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = Container.GetInstance<ShellView>();
            var regionManager = Container.GetInstance<RegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof (SplashScreenView));
            shell.Show();
            return shell;
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