//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCGIAY.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class REVIEW
    {
        public int PRODUCT_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public short RATING { get; set; }
        public string CONTENT { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}