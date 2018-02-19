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
    
    public partial class Ansatt
    {
        public Ansatt()
        {
            this.Ansatt1 = new HashSet<Ansatt>();
            this.AnsattMøteTid = new HashSet<AnsattMøteTid>();
            this.AnsattTilstedes = new HashSet<AnsattTilstede>();
            this.Avdelings = new HashSet<Avdeling>();
            this.Elevs = new HashSet<Elev>();
            this.Klasses = new HashSet<Klasse>();
            this.Sfoes = new HashSet<Sfo>();
        }
    
        public int Id { get; set; }
        public string Navn { get; set; }
        public decimal Stillingsstørrelse { get; set; }
        public string Tlfnr { get; set; }
        public int fk_AvdelingId { get; set; }
        public int fk_StillingsTypeId { get; set; }
        public Nullable<int> fk_VarslesAvAnsattId { get; set; }
    
        public virtual Avdeling Avdeling { get; set; }
        public virtual StillingsType StillingsType { get; set; }
        public virtual ICollection<Ansatt> Ansatt1 { get; set; }
        public virtual Ansatt Ansatt2 { get; set; }
        public virtual ICollection<AnsattMøteTid> AnsattMøteTid { get; set; }
        public virtual ICollection<AnsattTilstede> AnsattTilstedes { get; set; }
        public virtual ICollection<Avdeling> Avdelings { get; set; }
        public virtual ICollection<Elev> Elevs { get; set; }
        public virtual ICollection<Klasse> Klasses { get; set; }
        public virtual ICollection<Sfo> Sfoes { get; set; }
    }
}