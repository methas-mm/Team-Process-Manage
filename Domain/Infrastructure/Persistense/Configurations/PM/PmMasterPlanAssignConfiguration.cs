using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmMasterPlanAssignConfiguration : BaseConfiguration<PmMasterPlanAssign>
    {
        public override void Configure(EntityTypeBuilder<PmMasterPlanAssign> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MasterPlanAssignId });
        }
    }
}
