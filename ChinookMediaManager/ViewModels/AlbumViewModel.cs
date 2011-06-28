using ChinookMediaManager.Core.DynamicViewModel;
using ChinookMediaManager.Domain;

namespace ChinookMediaManager.ViewModels
{
    public class AlbumViewModel : ViewModelProxy<AlbumViewModel, Album>
    {
        
        
        public AlbumViewModel(Album album) : base(album)
        {
        
        }

        protected override void ConfigurePropertyMap()
        {
            AddProperty(p => p.Id);
            AddProperty(p => p.Title);
            AddProperty(p => p.Artist);
            AddProperty(p => p.LastPlayed);
        }

        
    }
}