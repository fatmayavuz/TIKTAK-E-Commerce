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
    
    public partial class Hizmetler_Egitim
    {
        public int id { get; set; }
        public string kategori_adi { get; set; }
        public string alt_kategori_adi { get; set; }
        public string hizmet_adi { get; set; }
        public Nullable<decimal> fiyat { get; set; }
        public string aciklama { get; set; }
    }
}
