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
    
    public partial class Art
    {
        public Art()
        {
            this.Tracks = new HashSet<Track>();
        }
    
        public System.Guid Id { get; set; }
        public string Mimetype { get; set; }
        public string Hash { get; set; }
    
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
