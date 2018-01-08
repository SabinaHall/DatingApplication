using System.ComponentModel.DataAnnotations;

namespace DatingApp.Models
{
    //Post-tabellen i databasen.
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Message { get; set; }

        //Beskriver realtionen i databasen. 
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}