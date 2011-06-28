using System;
using System.Collections.Generic;
using NHibernate;

namespace ChinookMediaManager.Domain
{
    public class AlbumRepository
    {
        private readonly ISession _session;
        
        public AlbumRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Album> GetAlbumList()
        {
            var albumList = _session.QueryOver<Album>().Fetch(x=>x.Artist).Eager.List();
            return albumList;
        }

        public void UpdateLastPlayed(int albumId)
        {
            _session.BeginTransaction();
            var album = _session.Get<Album>(albumId);
            album.LastPlayed = DateTime.Now;
            _session.SaveOrUpdate(album);
            _session.Flush();
            _session.Transaction.Commit();
            
        }
    }
}