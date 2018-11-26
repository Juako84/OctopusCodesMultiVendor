using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace OctopusCodesMultiVendor.Models
{
    [Table("CustomerVendors")]
    public partial class CustomerVendor
    {
        public CustomerVendor()
        {

        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }

        public DateTime DateCreate { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
