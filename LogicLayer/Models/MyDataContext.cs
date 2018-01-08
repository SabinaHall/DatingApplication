using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DatingApp.Models
{
    //Vår databas. 
    public class MyDataContext : DbContext
    {
        public MyDataContext() : base("JSFDatabase")
        {

        }

        //Beskriver för databasen hur vi vill att tabellerna ska se ut. 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)
                .WithRequired(x => x.From);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Posts)
               .WithRequired(x => x.Receiver);
            base.OnModelCreating(modelBuilder);
        }

        //Tabellerna vi vill skapa i databasen. 
        public DbSet<User> User { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }
}