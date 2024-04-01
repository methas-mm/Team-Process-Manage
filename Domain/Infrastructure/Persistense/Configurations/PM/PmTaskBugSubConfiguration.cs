using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmTaskBugSubConfiguration : BaseConfiguration<PmTaskBugSub>
    {
        public override void Configure(EntityTypeBuilder<PmTaskBugSub> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.TaskBugSubId });
        }
    }
}
