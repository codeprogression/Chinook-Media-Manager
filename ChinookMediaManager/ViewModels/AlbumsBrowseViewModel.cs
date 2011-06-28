using System.Collections.ObjectModel;
using System.Windows.Input;
using ChinookMediaManager.Core.DynamicViewModel;
using ChinookMediaManager.Domain;
using System.Linq;
using Microsoft.Practices.Prism.Commands;

namespace ChinookMediaManager.ViewModels
{
    public class AlbumsBrowseViewModel : CollectionViewModelProxy<AlbumViewModel,Album>
    {
        private readonly AlbumRepository _repository;

        public ICommand PlayAlbumCommand { get; set; }

        public AlbumsBrowseViewModel(AlbumRepository repository)
        {
            _repository = repository;
            PlayAlbumCommand = new DelegateCommand<AlbumViewModel>(PlayAlbumExecute, PlayAlbumCanExecute);
            Load();
        }

        protected override void ConfigurePropertyMap()
        {
            
        }

        protected override void Load()
        {
            var albums = _repository.GetAlbumList();
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
            _repository.UpdateLastPlayed(album.GetId());
            OnPropertyChanged("SelectedItem");
        }
    }
}