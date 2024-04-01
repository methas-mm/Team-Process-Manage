using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class StUserProfileConfiguration : BaseConfiguration<StUserProfile>
    {
        public override void Configure(EntityTypeBuilder<StUserProfile> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Id).HasColumnName("user_id");
            builder.HasKey(e => new { e.Id, e.ProfileCode });
        }
    }
}
