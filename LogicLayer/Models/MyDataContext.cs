using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //Beskriver för databasen hur vi vill att vänner-tabellen ska se ut. 
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)
                .WithRequired(x => x.From);
            base.OnModelCreating(modelBuilder);

            //Beskriver för databasen hur vi vill att post-tabellen ska se ut.
            modelBuilder.Entity<User>()
               .HasMany(x => x.Posts)
               .WithRequired(x => x.Receiver);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }
}