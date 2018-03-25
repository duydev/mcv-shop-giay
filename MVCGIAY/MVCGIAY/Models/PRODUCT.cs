//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCGIAY.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUCT
    {
        public int ID { get; set; }
        public int CATEGORY_ID { get; set; }
        public int SUPPLIER_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal PRICE { get; set; }
        public int QUANTITY { get; set; }
        public string IMAGE { get; set; }
        public Nullable<System.DateTime> CREATED_AT { get; set; }
        public Nullable<System.DateTime> UPDATED_AT { get; set; }
    
        public virtual CATEGORY CATEGORy { get; set; }
        public virtual SUPPLIER SUPPLIER { get; set; }
    }
}