//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class PlaylistTrack
    {
        public int Id { get; set; }
        public System.Guid Playlist { get; set; }
        public System.Guid Track { get; set; }
        public Nullable<int> Order { get; set; }
    
        public virtual Playlist Playlist1 { get; set; }
        public virtual Track Track1 { get; set; }
    }
}
