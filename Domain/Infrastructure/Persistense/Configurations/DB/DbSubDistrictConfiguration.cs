using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbSubDistrictConfiguration : BaseConfiguration<DbSubDistrict>
    {
        public override void Configure(EntityTypeBuilder<DbSubDistrict> builder)
        {
            base.Configure(builder);
            builder.HasKey(s => s.SubDistrictId);
        }
    }
}
