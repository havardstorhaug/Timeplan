//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Timeplan.BL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Avdeling
    {
        public Avdeling()
        {
            this.Ansatts = new HashSet<Ansatt>();
            this.Klasses = new HashSet<Klasse>();
        }
    
        public int Id { get; set; }
        public string Navn { get; set; }
        public Nullable<int> fk_AvdelingslederAnsattId { get; set; }
    
        public virtual ICollection<Ansatt> Ansatts { get; set; }
        public virtual Ansatt Ansatt { get; set; }
        public virtual ICollection<Klasse> Klasses { get; set; }
    }
}
