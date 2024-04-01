using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class StMenuConfiguration : BaseConfiguration<StMenu>
    {
        public override void Configure(EntityTypeBuilder<StMenu> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.MenuCode });
            builder.HasMany(e => e.SubMenus).WithOne().HasForeignKey(f => f.MainMenu).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.MenuLabels).WithOne().HasForeignKey(f => f.MenuCode).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
