using System.Windows.Input;
using System.Linq;
using ChinookMediaManager.Core.DynamicViewModel;
using ChinookMediaManager.Domain.Entities;
using Microsoft.Practices.Prism.Commands;
using NHibernate;

namespace ChinookMediaManager.Module.Albums
{
    public class AlbumsBrowseViewModel : CollectionViewModelProxy<AlbumViewModel,Album>
    {
        readonly ISession _session;

        public ICommand PlayAlbumCommand { get; set; }
        public AlbumsBrowseViewModel(ISession session)
        {
            _session = session;
            PlayAlbumCommand = new DelegateCommand<AlbumViewModel>(PlayAlbumExecute, PlayAlbumCanExecute);
            Load();
        }

        protected override void ConfigurePropertyMap()
        {
            
        }

        protected override void Load()
        {
            var albums =_session.QueryOver<Album>().Fetch(x=>x.Artist).Eager.List();
            Model.Clear();
            if (albums.Any())
                albums.Select(album=>new AlbumViewModel(album)).ToList().ForEach(Model.Add);
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
    }
}