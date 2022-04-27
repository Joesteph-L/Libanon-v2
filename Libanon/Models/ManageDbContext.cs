using Libanon.Models.EntityConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public class ManageDbContext:DbContext
    {
        public ManageDbContext() : base("name = DB")
        {
            Database.SetInitializer<ManageDbContext>(new DropCreateDatabaseIfModelChanges<ManageDbContext>());
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ISBN> ISBNs { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookEntityConfiguration());
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new RateEntityConfiguration());
            modelBuilder.Configurations.Add(new ISBNEntityConfiguration());

        }
    }
}