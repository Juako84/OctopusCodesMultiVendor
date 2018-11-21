using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("AccountVendor")]
    public partial class AccountVendor
    {
        public int Id { get; set; }
        //public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int VendorId { get; set; }
        public bool Status { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}
