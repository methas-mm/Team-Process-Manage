using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class StMenuProfileConfiguration : BaseConfiguration<StMenuProfile>
    {
        public override void Configure(EntityTypeBuilder<StMenuProfile> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ProfileCode, e.MenuCode });
        }
    }
}
