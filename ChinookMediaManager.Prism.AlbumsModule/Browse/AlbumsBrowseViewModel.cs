using System.ComponentModel;
using System.Linq;
using ChinookMediaManager.Domain.Entities;
using ChinookMediaManager.Prism.Core.DynamicViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using NHibernate;

namespace ChinookMediaManager.Prism.AlbumsModule.Browse
{
    public class AlbumsBrowseViewModel : CollectionViewModelProxy<AlbumViewModel,Album>, INavigationAware
    {
        readonly ISession _session;
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand<AlbumViewModel> PlayAlbumCommand { get; set; }
        public DelegateCommand OpenAlbumViewCommand { get; set; }
        
        public AlbumsBrowseViewModel(ISession session, IEventAggregator eventAggregator)
        {
            _session = session;
            _eventAggregator = eventAggregator;
            PlayAlbumCommand = new DelegateCommand<AlbumViewModel>(PlayAlbumExecute, PlayAlbumCanExecute);
            OpenAlbumViewCommand = new DelegateCommand(OpenAlbumView, CanOpenAlbumView);
            PropertyChanged += OnSelectedItemChanged; 
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

        private void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem" && SelectedItem != null)
                _eventAggregator.GetEvent<AlbumSelectedEvent>().Publish(SelectedItem);
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

        private void OpenAlbumView()
        {
            new AlbumDetails(SelectedItem).ShowDialog();
        }

        private bool CanOpenAlbumView()
        {
            return SelectedItem != null;
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
    public class AlbumSelectedEvent : CompositePresentationEvent<AlbumViewModel>
    {
    }
}