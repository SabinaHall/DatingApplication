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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Login>()
        //        .HasRequired(x => x.UserProfile)
        //        .WithMany()
        //        .WillCascadeOnDelete(false);
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> User { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}