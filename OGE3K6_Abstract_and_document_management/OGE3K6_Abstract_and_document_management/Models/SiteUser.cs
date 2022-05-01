using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGE3K6_Abstract_and_document_management.Models
{
    public class SiteUser : IdentityUser
    {
        [StringLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(200)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(200)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [StringLength(11)]
        [Display(Name = "Seal number")]
        public string SealNumber { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{(Title != "" ? Title : "")} {LastName} {FirstName}"; }
        }
    }
}
