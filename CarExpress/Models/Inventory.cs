
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
    
public partial class Inventory
{

    public int ID { get; set; }

    public Nullable<int> LocationID { get; set; }

    public Nullable<int> SupplierID { get; set; }

    public Nullable<int> ItemID { get; set; }

    public string ItemName { get; set; }

    public Nullable<double> CostPerUnit { get; set; }

    public string PurchasingUnit { get; set; }

    public Nullable<double> OpenStockLevel { get; set; }

    public Nullable<double> MinStockLevel { get; set; }

    public Nullable<double> CurrentStockLevel { get; set; }

    public string LastUpdatedBy { get; set; }

    public Nullable<System.DateTime> LastUpdatedDate { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<int> StatusId { get; set; }



    public virtual Inventory Inventory1 { get; set; }

    public virtual Inventory Inventory2 { get; set; }

    public virtual Item Item { get; set; }

    public virtual Location Location { get; set; }

    public virtual Supplier Supplier { get; set; }

}

}
