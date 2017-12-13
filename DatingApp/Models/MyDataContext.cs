using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.Entity;

namespace DatingApp.Models
{
    public class MyDataContext : DbContext
    {
        public MyDataContext() : base("JSFDatabase")
        {

        }

        public DbSet<UserProfile> UserProfiles{ get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}