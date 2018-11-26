using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("AccountCustomer")]
    public partial class AccountCustomer
    {
        public AccountCustomer()
        {
            //Messages = new HashSet<Message>();
            //Orderss = new HashSet<Orders>();
            //Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        //public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }

        public int CustomerId { get; set; }

        //public bool IsAdmin { get; set; }

        //public virtual ICollection<Message> Messages { get; set; }
        //public virtual ICollection<Orders> Orderss { get; set; }
        //public virtual ICollection<Review> Reviews { get; set; }
    }
}
