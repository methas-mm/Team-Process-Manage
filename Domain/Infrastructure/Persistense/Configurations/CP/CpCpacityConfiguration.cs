using Domain.Entities.CP;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.CP
{
   public class CpCpacityConfiguration : BaseConfiguration<CpCapacity>
    {
        public override void Configure(EntityTypeBuilder<CpCapacity> builder)
        {
            base.Configure(builder);
            builder.HasKey(d => d.CapacityId);
            builder.HasMany(e => e.CpCapacityDetail).WithOne().HasForeignKey(f => f.CapacityId);

        }

    }
}
