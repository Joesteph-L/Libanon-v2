using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Libanon.Models.EntityConfig
{
    public class BookEntityConfiguration:EntityTypeConfiguration<Book>
    {

        public BookEntityConfiguration()
        {
            this.ToTable("Books");
            this.HasKey<int>(b => b.BookId);

            this.HasRequired(i => i.CurrentISBN)
                .WithMany(b => b.Books)
                .HasForeignKey<int>(i => i.CurrentISBNId);
            
            this.HasRequired(u => u.CurrentOwner)
                .WithMany(b => b.MyBooks)
                .HasForeignKey<int>(u => u.CurrentOwnerId);

            this.HasOptional(u => u.CurrentBorrower)
                .WithMany(b => b.BorrowBooks)
                .HasForeignKey<int>(u => (int)u.CurrentBorrowerId)
                .WillCascadeOnDelete(false);

            this.HasMany(b => b.Borrowers)
                .WithRequired(b => b.CurrentBook)
                .HasForeignKey<int>(b => (int)b.CurrentBookId)
                .WillCascadeOnDelete(false);
        }
    }
}