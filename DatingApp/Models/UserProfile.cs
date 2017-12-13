using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; }

        [Display(Name = "Efternamn")]
        public string Lastname { get; set; }

        [Required]
        [Range(18, 99)]
        [Display(Name = "Ålder")]
        public int Age { get; set; }

        [ForeignKey("LoginId")]
        public virtual Login Login { get; set; }
    }
}