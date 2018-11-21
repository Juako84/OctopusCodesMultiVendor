using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            OrdersDetails = new HashSet<OrdersDetail>();
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public int Views { get; set; }

        public virtual Category Category { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
