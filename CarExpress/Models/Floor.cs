
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
    
public partial class Floor
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Floor()
    {

        this.Tables = new HashSet<Table>();

    }


    public int ID { get; set; }

    public Nullable<int> LocationID { get; set; }

    public string Title { get; set; }

    public string Name { get; set; }

    public string LastUpdatedBy { get; set; }

    public Nullable<System.DateTime> LastUpdatedDate { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<int> StatusID { get; set; }



    public virtual Location Location { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Table> Tables { get; set; }

}

}
