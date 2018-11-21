using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Photo")]
    public partial class Photo
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Main { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
