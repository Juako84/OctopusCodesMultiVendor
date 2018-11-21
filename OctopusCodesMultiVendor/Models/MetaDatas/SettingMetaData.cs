using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class SettingMetaData
    {
        [Required]
        public string Value { get; set; }
        
    }

    [ModelMetadataType(typeof(SettingMetaData))]
    public partial class Setting
    {

    }

}