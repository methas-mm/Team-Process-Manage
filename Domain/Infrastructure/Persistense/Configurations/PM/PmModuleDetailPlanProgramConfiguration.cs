using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmModuleDetailPlanProgramConfiguration : BaseConfiguration<PmModuleDetailPlanProgram>
    {
        public override void Configure(EntityTypeBuilder<PmModuleDetailPlanProgram> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ModuleDetailPlanProgramId });
        }
    }
}
