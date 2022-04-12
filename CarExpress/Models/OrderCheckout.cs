
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
    
public partial class OrderCheckout
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public OrderCheckout()
    {

        this.Table_orderCheckoutDetail = new HashSet<Table_orderCheckoutDetail>();

    }


    public int ID { get; set; }

    public Nullable<int> OrderID { get; set; }

    public Nullable<int> LocationID { get; set; }

    public Nullable<int> CustomerID { get; set; }

    public Nullable<int> TransactionNo { get; set; }

    public Nullable<int> OrderNo { get; set; }

    public Nullable<int> PaymentMode { get; set; }

    public Nullable<double> AmountPaid { get; set; }

    public Nullable<double> DiscountPercent { get; set; }

    public Nullable<int> DiscountType { get; set; }

    public Nullable<double> AmountDiscount { get; set; }

    public Nullable<double> AmountTotal { get; set; }

    public Nullable<double> GrandTotal { get; set; }

    public Nullable<double> Tax { get; set; }

    public string CardNumber { get; set; }

    public string CardHolderName { get; set; }

    public string CardType { get; set; }

    public Nullable<System.DateTime> CheckoutDate { get; set; }

    public string SessionID { get; set; }

    public Nullable<int> OrderStatus { get; set; }

    public string LastUpdateBy { get; set; }

    public Nullable<System.DateTime> LastUpdatedDate { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<double> ServiceCharges { get; set; }

    public Nullable<bool> IsPartial { get; set; }

    public Nullable<double> PartialAmount { get; set; }

    public string DiscountReason { get; set; }



    public virtual Customer Customer { get; set; }

    public virtual Location Location { get; set; }

    public virtual Order Order { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Table_orderCheckoutDetail> Table_orderCheckoutDetail { get; set; }

}

}