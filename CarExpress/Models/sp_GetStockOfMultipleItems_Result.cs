
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
    
public partial class sp_GetStockOfMultipleItems_Result
{

    public int ID { get; set; }

    public Nullable<int> LocationID { get; set; }

    public Nullable<int> ItemID { get; set; }

    public Nullable<double> StockIn { get; set; }

    public Nullable<double> StockOut { get; set; }

    public Nullable<double> RemainingStock { get; set; }

    public Nullable<double> MinStockLevel { get; set; }

    public Nullable<double> OpenStockLevel { get; set; }

    public Nullable<double> CurrentStockLevel { get; set; }

    public Nullable<double> ReceivedStockLevel { get; set; }

    public Nullable<double> ReturnStock { get; set; }

    public Nullable<System.DateTime> Date { get; set; }

    public string Description { get; set; }

    public Nullable<int> StatusId { get; set; }

    public Nullable<int> PurshaseOrderID { get; set; }

    public string PONum { get; set; }

}

}
