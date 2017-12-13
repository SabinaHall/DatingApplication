using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [StringLength(500)]
        public string Message { get; set; }

        [ForeignKey("SenderId")]
        public virtual UserProfile Sender { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual UserProfile Receiver { get; set; }
    }
}