using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class VendorMetaData
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

    [ModelMetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {

    }

}