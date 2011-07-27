using System;
using System.Collections.Generic;
using ChinookMediaManager.Domain.Entities;

namespace ChinookMediaManager.Prism.AlbumsModule.Design
{
    public class AlbumViewModel
    {
        public AlbumViewModel(int id, string title, string artist, DateTime lastPlayed)
        {
            Id = id;
            Title = title;
            ArtistName = artist;
            LastPlayed = lastPlayed;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public DateTime LastPlayed { get; set; }
        public IList<Track> Tracks { get; set; }
    }
}