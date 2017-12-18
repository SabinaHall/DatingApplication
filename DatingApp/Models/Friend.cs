using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatingApp.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public virtual User From { get; set; }
        public virtual User To { get; set; }
        public bool IsFriend { get; set; }
    }
}