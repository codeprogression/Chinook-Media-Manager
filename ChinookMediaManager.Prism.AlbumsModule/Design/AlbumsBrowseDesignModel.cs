using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.Prism.AlbumsModule.Design
{
    public class AlbumsBrowseViewModel 
    {
        public DelegateCommand<Browse.AlbumViewModel> PlayAlbumCommand { get; set; }
        public DelegateCommand OpenAlbumViewCommand { get; set; }


        public ObservableCollection<AlbumViewModel> Model
        {
            get
            {
                return new ObservableCollection<AlbumViewModel>
                {
                    new AlbumViewModel(1, "Title1", "Artist1", DateTime.Now),
                    new AlbumViewModel(2, "Title2", "Artist2", DateTime.Now),
                    new AlbumViewModel(3, "Title3", "Artist3", DateTime.Now),
                    new AlbumViewModel(4, "Title4", "Artist4", DateTime.Now),
                    new AlbumViewModel(5, "Title5", "Artist5", DateTime.Now)
                };
            }
        }

        protected void Load()
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}