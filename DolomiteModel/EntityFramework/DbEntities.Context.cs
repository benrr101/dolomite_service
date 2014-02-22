﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DolomiteModel.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class DbEntities : DbContext
    {
        public DbEntities()
            : base("name=DbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Album> Albums { get; set; }
        public DbSet<Art> Arts { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<AutoplaylistRule> AutoplaylistRules { get; set; }
        public DbSet<Autoplaylist> Autoplaylists { get; set; }
        public DbSet<AvailableQuality> AvailableQualities { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<MetadataField> MetadataFields { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<Quality> Qualities { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserKey> UserKeys { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Session> Sessions { get; set; }
    
        public virtual ObjectResult<Nullable<System.Guid>> GetAndLockTopOnboardingItem()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("GetAndLockTopOnboardingItem");
        }
    
        public virtual int ReleaseAndCompleteOnboardingItem(Nullable<System.Guid> workItem)
        {
            var workItemParameter = workItem.HasValue ?
                new ObjectParameter("workItem", workItem) :
                new ObjectParameter("workItem", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReleaseAndCompleteOnboardingItem", workItemParameter);
        }
    
        public virtual int ResetOnboardingStatus(Nullable<System.Guid> guid)
        {
            var guidParameter = guid.HasValue ?
                new ObjectParameter("guid", guid) :
                new ObjectParameter("guid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetOnboardingStatus", guidParameter);
        }
    
        public virtual int SetTrackHash(Nullable<System.Guid> trackId, string hash)
        {
            var trackIdParameter = trackId.HasValue ?
                new ObjectParameter("trackId", trackId) :
                new ObjectParameter("trackId", typeof(System.Guid));
    
            var hashParameter = hash != null ?
                new ObjectParameter("hash", hash) :
                new ObjectParameter("hash", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetTrackHash", trackIdParameter, hashParameter);
        }
    
        public virtual int IncrementPlaylistTrackOrder(Nullable<System.Guid> playlist, Nullable<int> position)
        {
            var playlistParameter = playlist.HasValue ?
                new ObjectParameter("playlist", playlist) :
                new ObjectParameter("playlist", typeof(System.Guid));
    
            var positionParameter = position.HasValue ?
                new ObjectParameter("position", position) :
                new ObjectParameter("position", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("IncrementPlaylistTrackOrder", playlistParameter, positionParameter);
        }
    
        public virtual int DecrementPlaylistTrackOrder(Nullable<System.Guid> playlist, Nullable<int> position)
        {
            var playlistParameter = playlist.HasValue ?
                new ObjectParameter("playlist", playlist) :
                new ObjectParameter("playlist", typeof(System.Guid));
    
            var positionParameter = position.HasValue ?
                new ObjectParameter("position", position) :
                new ObjectParameter("position", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DecrementPlaylistTrackOrder", playlistParameter, positionParameter);
        }
    
        public virtual int ReleaseAndCompleteMetadataUpdate(Nullable<System.Guid> workItem)
        {
            var workItemParameter = workItem.HasValue ?
                new ObjectParameter("workItem", workItem) :
                new ObjectParameter("workItem", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReleaseAndCompleteMetadataUpdate", workItemParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> GetAndLockTopMetadataItem()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("GetAndLockTopMetadataItem");
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> GetAndLockTopArtItem()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("GetAndLockTopArtItem");
        }
    
        public virtual int ReleaseAndCompleteArtChange(Nullable<System.Guid> workItem)
        {
            var workItemParameter = workItem.HasValue ?
                new ObjectParameter("workItem", workItem) :
                new ObjectParameter("workItem", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReleaseAndCompleteArtChange", workItemParameter);
        }
    }
}
