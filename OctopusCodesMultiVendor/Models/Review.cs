using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Review")]
    public partial class Review
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string Detail { get; set; }
        public DateTime DatePost { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
