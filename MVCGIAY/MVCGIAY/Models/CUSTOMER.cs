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
    
    public partial class CUSTOMER
    {
        public CUSTOMER()
        {
            this.REVIEWS = new HashSet<REVIEW>();
        }
    
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string FULLNAME { get; set; }
        public string GENDER { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public Nullable<System.DateTime> CREATED_AT { get; set; }
        public Nullable<System.DateTime> UPDATED_AT { get; set; }
    
        public virtual ICollection<REVIEW> REVIEWS { get; set; }
    }
}
