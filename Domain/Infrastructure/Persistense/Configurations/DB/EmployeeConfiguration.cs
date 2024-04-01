using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class EmployeeConfiguration : BaseConfiguration<DbEmployee>
    {
        public override void Configure(EntityTypeBuilder<DbEmployee> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new {  e.EmployeeCode });
            


        }
    }
}
