using ChinookMediaManager.Domain.Entities;
using ChinookMediaManager.Prism.AlbumsModule.Browse;
using ChinookMediaManager.Prism.Core.DynamicViewModel;
using ChinookMediaManager.Prism.Core.Extensions;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Windows.Controls;

namespace ChinookMediaManager.Prism.AlbumsModule.Toolbar
{
    public class AlbumToolbarButtonViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private AlbumViewModel _selectedAlbum;
        public DelegateCommand OpenAlbumsBrowseViewCommand { get; set; }
        public DelegateCommand OpenAlbumViewCommand { get; set; }

        public AlbumToolbarButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            OpenAlbumsBrowseViewCommand = new DelegateCommand(OpenAlbumsBrowseView, CanOpenAlbumsBrowseView);
            OpenAlbumViewCommand = new DelegateCommand(OpenAlbumView, CanOpenAlbumView);
            eventAggregator.GetEvent<AlbumSelectedEvent>().Subscribe(OnAlbumSelected);
        }

        private void OpenAlbumView()
        {
            MessageBox.Show(string.Format("Opened {0} by {1}", SelectedAlbum.Model.Title, SelectedAlbum.Model.Artist.Name));
        }

        private void OnAlbumSelected(AlbumViewModel album)
        {
            SelectedAlbum = album;
        }

        protected AlbumViewModel SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                RaisePropertyChanged("SelectedAlbum");
                OpenAlbumViewCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanOpenAlbumView()
        {
            return SelectedAlbum != null;
        }
        private bool CanOpenAlbumsBrowseView()
        {
            return true;
        }

        private void OpenAlbumsBrowseView()
        {
            _regionManager.Navigate<AlbumsBrowseView>("ContentRegion");
            OpenAlbumsBrowseViewCommand.RaiseCanExecuteChanged();
            OpenAlbumViewCommand.RaiseCanExecuteChanged();
        }


    }
}