using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Epost")]
        public string Email { get; set; }
        [Required]
        [Display (Name = "Lösenord")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Invalid")]
        public string Password { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}