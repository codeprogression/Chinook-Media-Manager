using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ChinookMediaManager.Domain
{
    public class Album : Entity
    {
        private readonly IList<Track> tracks;

        public Album()
        {
            tracks = new List<Track>();
        }

        public virtual Artist Artist { get; set; }

        public virtual string Title { get; set; }

        public virtual DateTime? LastPlayed { get; set; }


        public virtual IList<Track> Tracks
        {
            get { return tracks; }
        }

        public virtual void AddTrack(Track track)
        {
            track.Album = this;
            Tracks.Add(track);
        }

        public virtual void RemoveTrack(Track track)
        {
            track.Album = null;
            Tracks.Remove(track);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}