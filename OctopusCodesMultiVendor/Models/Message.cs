using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Message")]
    public partial class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreation { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public bool Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
