using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace OctopusCodesMultiVendor.Models
{
    [Table("Users")]
    public partial class Users
    {
        public Users()
        {
            
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }       
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
    }
}
