using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class MemberShipMetaData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Month { get; set; }

    }

    [ModelMetadataType(typeof(MemberShipMetaData))]
    public partial class MemberShip
    {

    }

}