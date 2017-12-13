using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class Login
    {
       
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Epost")]
        public string Email { get; set; }

        [Required]
        [Display (Name = "Lösenord")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Invalid")]
        public string Password { get; set; }

        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}