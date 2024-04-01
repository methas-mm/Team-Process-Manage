using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.PM
{
    public class PmTaskWorkConfiguration : BaseConfiguration<PmTaskWork>
    {
        public override void Configure(EntityTypeBuilder<PmTaskWork> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.TaskWorkId });
        }
    }
}
