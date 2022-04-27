using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Libanon.Models.EntityConfig
{
    public class BorrowerEntityConfiguration : EntityTypeConfiguration<Borrower>
    {
        public BorrowerEntityConfiguration()
        {
            this.ToTable("Borrower");
            this.HasKey<int>(i => i.BorrowerId);
        }
    }
}