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
    
    public partial class Sfo
    {
        public Sfo()
        {
            this.Elevs = new HashSet<Elev>();
            this.Ansatts = new HashSet<Ansatt>();
        }
    
        public int Id { get; set; }
        public string Navn { get; set; }
        public int fk_TidsInndelingId { get; set; }
        public int fk_TidligvaktTidsInndelingId { get; set; }
        public int fk_SeinvaktTidsInndelingId { get; set; }
    
        public virtual ICollection<Elev> Elevs { get; set; }
        public virtual TidsInndeling TidsInndeling { get; set; }
        public virtual TidsInndeling TidsInndeling1 { get; set; }
        public virtual TidsInndeling TidsInndeling2 { get; set; }
        public virtual ICollection<Ansatt> Ansatts { get; set; }
    }
}
