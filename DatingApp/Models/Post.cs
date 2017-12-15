using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class Post
    {
     
        public int PostId { get; set; }
        [StringLength(500)]
        public string Message { get; set; }

        public virtual User Sender { get; set; }
    
        //public virtual User Receiver { get; set; }
    }
}