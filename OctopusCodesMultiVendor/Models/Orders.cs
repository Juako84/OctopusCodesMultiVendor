using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Orders")]
    public partial class Orders
    {
        public Orders()
        {
            OrdersDetails = new HashSet<OrdersDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreation { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public int OrderStatusId { get; set; }
        public int? PaymentId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
    }
}
