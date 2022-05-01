using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace OGE3K6_Abstract_and_document_management.Models
{
    public class Abstract
    {
        [Key]
        [Display(Name = "ID")]
        public string UID { get; set; }

        [StringLength(150)]
        [Required]
        [Display(Name = "Title of the abstract")]
        public string AbstractTitle { get; set; }

        [StringLength(2500)]
        [Required]
        [Display(Name = "The abstract")]
        public string AbstractText { get; set; }

        [Display(Name = "Upload date")]
        [Required]
        public DateTime UploadTime { get; set; }

        [Display(Name = "Uploader")]
        [Required]
        public virtual IdentityUser Uploader { get; set; }

        [Display(Name = "Reviewer")]
        public virtual IdentityUser Reviewer { get; set; }

        [Display(Name = "Review date")]
        public DateTime ReviewedAt { get; set; }

        [Display(Name = "Status of the abstract")]
        public AbstractStatus Status { get; set; }
    }
}
