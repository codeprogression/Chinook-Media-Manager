using System;
using ChinookMediaManager.Prism.AlbumsModule.Browse;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System.Linq;

namespace ChinookMediaManager.Prism.AlbumsModule.Toolbar
{
    public class AlbumToolbarButtonViewModel 
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand OpenAlbumsBrowseViewCommand { get; set; }

        public AlbumToolbarButtonViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            OpenAlbumsBrowseViewCommand =
                new DelegateCommand(OpenAlbumsBrowseView, CanOpenAlbumsBrowseView);
        }

        private bool CanOpenAlbumsBrowseView()
        {
            return true;
            return _regionManager.Regions["ContentRegion"].ActiveViews.All(x => !(x is AlbumsBrowseView));
        }

        private void OpenAlbumsBrowseView()
        {
            _regionManager.RequestNavigate("ContentRegion", new Uri(typeof (AlbumsBrowseView).Name, UriKind.Relative));
            OpenAlbumsBrowseViewCommand.RaiseCanExecuteChanged();
        }
    }
}