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
    
    public partial class Mark
    {
        public int Marks_ID { get; set; }
        public string Mark_Obtained { get; set; }
        public string Mark_Description { get; set; }
        public string Sympol { get; set; }
        public Nullable<int> Test_ID { get; set; }
        public Nullable<int> Student_ID { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Test Test { get; set; }
    }
}
