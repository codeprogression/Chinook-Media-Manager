using System;
using ChinookMediaManager.Core.DynamicViewModel;
using ChinookMediaManager.Domain;
using NHibernate;

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

        public void UpdatePlayed(ISession session)
        {
            ((dynamic)this).LastPlayed = DateTime.UtcNow;
            Update(session);
        }
    }
}