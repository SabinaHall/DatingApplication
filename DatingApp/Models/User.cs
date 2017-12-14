using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; }

        [Required]
        [Range(18, 99)]
        [Display(Name = "Ålder")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Epost, användarnamn")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Lösenord")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Invalid")]
        public string Password { get; set; }
    }
}