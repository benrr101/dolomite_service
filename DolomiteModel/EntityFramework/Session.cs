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
    
    public partial class Session
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int ApiKey { get; set; }
        public System.DateTime IdleTimeout { get; set; }
        public System.DateTime AbsoluteTimeout { get; set; }
        public int User { get; set; }
    
        public virtual ApiKey ApiKey1 { get; set; }
        public virtual User User1 { get; set; }
    }
}
