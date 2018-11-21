using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("MemberShip")]
    public partial class MemberShip
    {
        public MemberShip()
        {
            MemberShipVendors = new HashSet<MemberShipVendor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Month { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<MemberShipVendor> MemberShipVendors { get; set; }
    }
}
