using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Libanon.Models.EntityConfig
{
    public class UserEntityConfiguration:EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            this.ToTable("Users");
            this.HasKey<int>(u => u.UserId);
            
            
        }
    }
}