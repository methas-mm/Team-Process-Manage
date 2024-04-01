using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmMasterPlanConfiguration : BaseConfiguration<PmMasterPlan>
    {
        public override void Configure(EntityTypeBuilder<PmMasterPlan> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MasterPlanId });
            builder.HasMany(e => e.PmMasterPlanAssign).WithOne().HasForeignKey(f => f.MasterPlanId);
        }
    }
}
