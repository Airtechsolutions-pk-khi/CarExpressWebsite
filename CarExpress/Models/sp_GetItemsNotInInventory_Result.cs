
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
    
public partial class sp_GetItemsNotInInventory_Result
{

    public int ID { get; set; }

    public Nullable<int> SubCategoryID { get; set; }

    public Nullable<int> UnitID { get; set; }

    public string Name { get; set; }

    public string NameOnReceipt { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public string Barcode { get; set; }

    public string SKU { get; set; }

    public Nullable<int> DisplayOrder { get; set; }

    public Nullable<bool> SortByAlpha { get; set; }

    public Nullable<double> Price { get; set; }

    public Nullable<double> Cost { get; set; }

    public string ItemType { get; set; }

    public string LastUpdatedBy { get; set; }

    public Nullable<System.DateTime> LastUpdatedDate { get; set; }

    public Nullable<int> StatusID { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<bool> HasVariant { get; set; }

}

}
