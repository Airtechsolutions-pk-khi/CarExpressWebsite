
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
    
public partial class sp_GetAdminOrders_api_Result
{

    public int ID { get; set; }

    public Nullable<int> CustomerID { get; set; }

    public Nullable<int> LocationID { get; set; }

    public Nullable<int> TransactionNo { get; set; }

    public Nullable<int> OrderNo { get; set; }

    public Nullable<int> TableNo { get; set; }

    public Nullable<int> WaiterNo { get; set; }

    public Nullable<int> OrderTakerID { get; set; }

    public Nullable<System.DateTime> OrderCreatedDT { get; set; }

    public string OrderType { get; set; }

    public Nullable<int> GuestCount { get; set; }

    public string DeliveryAddress { get; set; }

    public Nullable<int> AgentID { get; set; }

    public string AgentName { get; set; }

    public string DeliveryTime { get; set; }

    public Nullable<double> Points { get; set; }

    public string OrderMode { get; set; }

    public Nullable<int> StatusID { get; set; }

    public string SessionID { get; set; }

    public string LastUpdateBy { get; set; }

    public Nullable<System.DateTime> LastUpdateDT { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<bool> IsAvailiable { get; set; }

    public string CounterType { get; set; }

}

}
