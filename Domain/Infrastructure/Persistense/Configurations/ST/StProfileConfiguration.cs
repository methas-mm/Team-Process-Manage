using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class StProfileConfiguration : BaseConfiguration<StProfile>
    {
        public override void Configure(EntityTypeBuilder<StProfile> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ProfileCode });
            builder.HasMany(e => e.MenuProfiles).WithOne().HasForeignKey(f => f.ProfileCode).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
