using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Models
{
    //Våran tabell för användare. 
    public class User
    { 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Invalid")]
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

        public bool IsVisible { get; set; }

        public string Filename { get; set; }

        public string ContentType { get; set; }

        public byte[] File { get; set; }

        public string SId { get; set; }

        public bool IsPicVisible { get; set; }

        //Beskriver relationerna i databasen. Här att en användare kan ha många posts och följare. 
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }

    }
}