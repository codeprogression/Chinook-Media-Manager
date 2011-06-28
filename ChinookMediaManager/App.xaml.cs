using System.Windows;
using ChinookMediaManager.Core.Bootstrap;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace ChinookMediaManager
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NHibernateProfiler.Initialize();
            base.OnStartup(e);
            new Bootstrapper().Run();
        }
    }
}
