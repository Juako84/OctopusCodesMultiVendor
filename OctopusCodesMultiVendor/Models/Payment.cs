using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Payment")]
    public partial class Payment
    {
        public Payment()
        {
            Orderss = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Orders> Orderss { get; set; }
    }
}
