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
    
    public partial class Vasıta_Kategoriler
    {
        public int id { get; set; }
        public Nullable<int> vasıta_id { get; set; }
        public Nullable<int> kategori_id { get; set; }
    
        public virtual Kategoriler Kategoriler { get; set; }
        public virtual Vasıta Vasıta { get; set; }
    }
}
