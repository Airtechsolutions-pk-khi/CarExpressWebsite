
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace CarExpress.Models
{

using System;
    using System.Collections.Generic;
    
public partial class CustomerAddress
{

    public int CustomerAddressID { get; set; }

    public string Address { get; set; }

    public string NickName { get; set; }

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public Nullable<int> StatusID { get; set; }

    public string StreetName { get; set; }

    public Nullable<int> CustomerID { get; set; }

    public string Country { get; set; }

    public string ContactNo { get; set; }



    public virtual Customer Customer { get; set; }

}

}
