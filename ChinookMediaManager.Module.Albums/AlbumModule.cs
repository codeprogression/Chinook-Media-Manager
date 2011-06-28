using ChinookMediaManager.Core.Prism;
using ChinookMediaManager.Module.Albums.Browse;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.Module.Albums
{
    public class AlbumModule : IChinookModule
    {
        public AlbumModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof (AlbumsBrowseView));
        }

        public void Initialize()
        {
        }
    }
}