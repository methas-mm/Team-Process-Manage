using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmModuleDetailPlanProgramAssignConfiguration : BaseConfiguration<PmModuleDetailPlanProgramAssign>
    {
        public override void Configure(EntityTypeBuilder<PmModuleDetailPlanProgramAssign> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ModuleDetailPlanProgramAssignId });
        }
    }
}
