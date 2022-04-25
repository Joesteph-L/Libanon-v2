using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Libanon.Models.EntityConfig
{
    public class RateEntityConfiguration:EntityTypeConfiguration<Rate>
    {
        public RateEntityConfiguration() 
        {
            this.ToTable("Rate");
            this.HasKey<int>(r => r.RateId);
            this.HasRequired(i => i.CurrentISBN)
                .WithMany(r => r.Rates)
                .HasForeignKey<int>(i => i.CurrentISBNId);
        }
    }
}