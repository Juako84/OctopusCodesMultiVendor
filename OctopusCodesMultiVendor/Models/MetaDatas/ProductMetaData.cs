using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class ProductMetaData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

    }

    [ModelMetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }

}