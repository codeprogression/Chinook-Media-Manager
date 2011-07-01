using System;
using ChinookMediaManager.Core.DynamicViewModel;
using ChinookMediaManager.Domain.Entities;
using NHibernate;

namespace ChinookMediaManager.Prism.AlbumsModule.Browse
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

        public void UpdatePlayed(ISession session)
        {
            Model.LastPlayed = DateTime.UtcNow;
            Update(session);
        }
    }
}