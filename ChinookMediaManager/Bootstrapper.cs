using System;
using System.Collections.Generic;
using System.Windows;
using ChinookMediaManager.Infrastructure;
using ChinookMediaManager.Infrastructure.Persistence;
using ChinookMediaManager.Views;

using NHibernate;

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

        protected override void ConfigureContainer()
        {
            Container.Configure(x =>
            {
                x.AddRegistry<NHibernateRegistry>();
//                x.AddRegistry<ModuleRegistry>();
            });
            base.ConfigureContainer();

        }
    }
}