using System.Collections.Generic;
using System.ComponentModel;
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
        IList<Album> albums;
        public DelegateCommand<AlbumViewModel> PlayAlbumCommand { get; set; }
        
        
        bool _isReady;
        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                OnPropertyChanged("IsReady");
            }
        }
        
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
            IsReady = false;
            var albums = _session.QueryOver<Album>().Fetch(x => x.Artist).Eager.List();
            Model.Clear();
            if (albums.Any())
                albums.Select(album => new AlbumViewModel(album)).ToList().ForEach(Model.Add);
            IsReady = true;
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