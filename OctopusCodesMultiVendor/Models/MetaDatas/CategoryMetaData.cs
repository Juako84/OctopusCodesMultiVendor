using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class CategoryMetaData
    {
        [Required]
        public string Name { get; set; }

    }

    [ModelMetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }

}