using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbProvinceConfiguration : BaseConfiguration<DbProvince>
    {
        public override void Configure(EntityTypeBuilder<DbProvince> builder)
        {
            base.Configure(builder);
            builder.HasKey(p => p.ProvinceId);
        }
    }
}
