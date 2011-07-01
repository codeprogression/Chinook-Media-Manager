using System.Linq;
using ChinookMediaManager.Domain.Entities;
using ChinookMediaManager.Prism.Core.DynamicViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using NHibernate;

namespace ChinookMediaManager.Prism.AlbumsModule.Browse
{
    public class AlbumsBrowseViewModel : CollectionViewModelProxy<AlbumViewModel,Album>, INavigationAware
    {
        readonly ISession _session;
        public DelegateCommand<AlbumViewModel> PlayAlbumCommand { get; set; }
        
        public AlbumsBrowseViewModel(ISession session)
        {
            _session = session;
            PlayAlbumCommand = new DelegateCommand<AlbumViewModel>(PlayAlbumExecute, PlayAlbumCanExecute);
        }

        protected override void ConfigurePropertyMap()
        {
            
        }

        protected override void Load()
        {
            var albums = _session.QueryOver<Album>().Fetch(x => x.Artist).Eager.List();
            Model.Clear();
            if (albums.Any())
                albums.Select(album => new AlbumViewModel(album)).ToList().ForEach(Model.Add);
        }

        private bool PlayAlbumCanExecute(AlbumViewModel album)
        {
            return true;
        }

        private void PlayAlbumExecute(AlbumViewModel album)
        {
            album.UpdatePlayed(_session);
            _session.Transaction.Commit();
            _session.BeginTransaction();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
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