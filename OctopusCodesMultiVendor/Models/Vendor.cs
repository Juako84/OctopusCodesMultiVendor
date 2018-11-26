using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Vendor")]
    public partial class Vendor
    {
        public Vendor()
        {
            Categories = new HashSet<Category>();
            MemberShipVendors = new HashSet<MemberShipVendor>();
            Messages = new HashSet<Message>();
            Orderss = new HashSet<Orders>();
            Products = new HashSet<Product>();
            Reviews = new HashSet<Review>();
            AccountVendors = new HashSet<AccountVendor>();
            CustomerVendors = new HashSet<CustomerVendor>();
        }

        public int Id { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<MemberShipVendor> MemberShipVendors { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Orders> Orderss { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<AccountVendor> AccountVendors { get; set; }

        public virtual ICollection<CustomerVendor> CustomerVendors { get; set; }
    }
}
