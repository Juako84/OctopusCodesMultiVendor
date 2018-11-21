using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            InverseParents = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int? ParentId { get; set; }
        public int? VendorId { get; set; }

        public virtual Category Parent { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<Category> InverseParents { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
