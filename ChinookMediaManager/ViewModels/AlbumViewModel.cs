using ChinookMediaManager.Domain;
using ChinookMediaManager.Infrastructure.DynamicViewModel;

namespace ChinookMediaManager.ViewModels
{
    public class AlbumViewModel : DynamicViewModelBase<AlbumViewModel, Album>
    {
        public AlbumViewModel(Album album) : this()
        {
            WrappedEntity = album;
        }

        public AlbumViewModel()
        {
            AddProperty(p=>p.Id);
            AddProperty(p=>p.Title);
            AddProperty(p => p.Artist);
            AddProperty(p => p.LastPlayed);
        }
    }
}