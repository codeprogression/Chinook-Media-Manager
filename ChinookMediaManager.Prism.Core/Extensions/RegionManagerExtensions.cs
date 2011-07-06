using System;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.Prism.Core.Extensions
{
    public static class RegionManagerExtensions
    {
        public static void Navigate<T>(this IRegionManager regionManager, string regionName)
        {
            regionManager.RequestNavigate(regionName, typeof (T).FullName);
        }

        public static void Navigate(this IRegionManager regionManager, string regionName, Type view)
        {
            regionManager.RequestNavigate(regionName, view.FullName);
        }

        public static void Navigate(this IRegionManager regionManager, string regionName, string viewName)
        {
            regionManager.RequestNavigate(regionName, viewName);
        }
    }
}