using System.Collections.ObjectModel;
using System.Windows.Input;
using ChinookMediaManager.Domain;
using Microsoft.Practices.Prism.Commands;
using System.Linq;

namespace ChinookMediaManager.ViewModels
{
    public class AlbumsBrowseViewModel : ObservableCollection<AlbumViewModel>
    {
        private readonly AlbumRepository _repository;

        public ICommand PlayAlbumCommand { get; set; }
        public ObservableCollection<AlbumViewModel> Subject { get; set; }

        public AlbumsBrowseViewModel(AlbumRepository repository)
        {
            _repository = repository;
            InitializeModel();
            
        }

        private void InitializeModel()
        {
            Subject = new ObservableCollection<AlbumViewModel>(_repository.GetAlbumList().Select(album=>new AlbumViewModel(album)));
            PlayAlbumCommand = new DelegateCommand<object>(PlayAlbumExecute, PlayAlbumCanExecute);
        }
        
        private bool PlayAlbumCanExecute(object arg)
        {
            return true;
        }

        private void PlayAlbumExecute(object obj)
        {
            _repository.UpdateLastPlayed((int)obj);
        }

    }
}