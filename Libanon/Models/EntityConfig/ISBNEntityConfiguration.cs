using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Libanon.Models.EntityConfig
{
    public class ISBNEntityConfiguration:EntityTypeConfiguration<ISBN>
    {
        public ISBNEntityConfiguration()
        {
            this.ToTable("ISBN");
            this.HasKey<int>(i => i.ISBNId);
        }
    }
}