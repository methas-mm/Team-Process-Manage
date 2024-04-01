using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    class DbDistrictConfiguration : BaseConfiguration<DbDistrict>
    {
        public override void Configure(EntityTypeBuilder<DbDistrict> builder)
        {
            base.Configure(builder);
            builder.HasKey(d => d.DistrictId);
        }
    }
}
