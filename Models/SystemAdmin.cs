//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace INF272AdminSubsystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemAdmin
    {
        public int Admin_ID { get; set; }
        public Nullable<int> SystemUser_ID { get; set; }
    
        public virtual SystemUser SystemUser { get; set; }
    }
}
