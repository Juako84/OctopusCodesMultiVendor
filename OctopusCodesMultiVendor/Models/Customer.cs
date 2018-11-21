using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            Messages = new HashSet<Message>();
            Orderss = new HashSet<Orders>();
            Reviews = new HashSet<Review>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Code { get; set; }

        public string Phone { get; set; }

        public bool Status { get; set; }

        public string Rut { get; set; }

        public string RutDv { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Orders> Orderss { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
