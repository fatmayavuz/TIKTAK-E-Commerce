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
    
    public partial class Odemeler
    {
        public int id { get; set; }
        public int kullanici_id { get; set; }
        public Nullable<int> ilan_id { get; set; }
        public decimal tutar { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
    
        public virtual Ilanlar Ilanlar { get; set; }
        public virtual Kullanıcılar Kullanıcılar { get; set; }
    }
}
