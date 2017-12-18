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

        //För entiten bok, som måste ha en genre och genre i sin tur har många böcker, 
        //när du tar bort en genre ta inte bort böckerna.
        //Går att göra detta med Data Annotations. 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)
                .WithMany();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Posts { get; set; }

        //public DbSet<User> Friends { get; set; }
    }
}