using System.ComponentModel.DataAnnotations.Schema;

namespace OctopusCodesMultiVendor.Models
{
    [Table("Page")]
    public partial class Page
    {
        public int Id { get; set; }
        public string Plug { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }
    }
}
