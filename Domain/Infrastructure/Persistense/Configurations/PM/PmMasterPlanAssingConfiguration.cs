using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
   public class PmMasterPlanAssingConfiguration : BaseConfiguration<PmMasterPlanAssing>
    {
        public override void Configure(EntityTypeBuilder<PmMasterPlanAssing> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MasterPlanAssingId });
        }
    }
}
