using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Models.MetaDatas
{
    public class AccountMetaData
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    [ModelMetadataType(typeof(AccountMetaData))]
    public partial class Account
    {
    }

}