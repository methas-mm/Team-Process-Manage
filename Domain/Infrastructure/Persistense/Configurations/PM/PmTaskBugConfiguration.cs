using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmTaskBugConfiguration : BaseConfiguration<PmTaskBug>
    {
        public override void Configure(EntityTypeBuilder<PmTaskBug> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.TaskBugId });
            builder.HasMany(e => e.PmTaskBugSubs).WithOne().HasForeignKey(f => f.TaskBugId);
        }
    }
}
