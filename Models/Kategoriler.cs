//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bitirme_database_new.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kategoriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoriler()
        {
            this.Emlak = new HashSet<Emlak>();
            this.Giyim_Taki = new HashSet<Giyim_Taki>();
            this.Ilanlar = new HashSet<Ilanlar>();
            this.Vasıta_Kategoriler = new HashSet<Vasıta_Kategoriler>();
        }
    
        public int id { get; set; }
        public string ana_kategori { get; set; }
        public string alt_kategori { get; set; }
        public string aciklama { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emlak> Emlak { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Giyim_Taki> Giyim_Taki { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ilanlar> Ilanlar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vasıta_Kategoriler> Vasıta_Kategoriler { get; set; }
    }
}
