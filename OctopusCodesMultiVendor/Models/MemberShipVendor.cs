using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("MemberShipVendor")]
    public partial class MemberShipVendor
    {
        public int Id { get; set; }
        public int MemerShipId { get; set; }
        public int VendorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public virtual MemberShip MemberShip { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
