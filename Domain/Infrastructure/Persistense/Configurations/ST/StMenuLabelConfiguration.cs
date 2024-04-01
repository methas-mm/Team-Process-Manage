using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class StMenuLabelConfiguration : BaseConfiguration<StMenuLabel>
    {
        public override void Configure(EntityTypeBuilder<StMenuLabel> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MenuCode, e.LangCode });
        }
    }
}
