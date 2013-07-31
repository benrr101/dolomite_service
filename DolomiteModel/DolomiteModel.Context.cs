﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DolomiteModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<AutoplaylistRule> AutoplaylistRules { get; set; }
        public DbSet<Autoplaylist> Autoplaylists { get; set; }
        public DbSet<AvailableQuality> AvailableQualities { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<MetadataField> MetadataFields { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<Quality> Qualities { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<Nullable<System.Guid>> GetAndLockTopOnboardingItem()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("GetAndLockTopOnboardingItem");
        }
    }
}
