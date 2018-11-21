namespace OctopusCodesMultiVendor.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string Photo { get; set; }

        public decimal Price { get; set; }


        public int Quantity { get; set; }
    }
}