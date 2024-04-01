using Domain.Entities.CP;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.CP
{
   public class CpCpacityDetailConfiguration : BaseConfiguration<CpCapacityDetail>
    {
        public override void Configure(EntityTypeBuilder<CpCapacityDetail> builder)
        {
            base.Configure(builder);
            builder.HasKey(d => d.CapacityDetailId);
        }
    }
}
