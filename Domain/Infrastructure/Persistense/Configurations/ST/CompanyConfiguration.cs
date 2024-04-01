using Domain.Entities.ST;
using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.SU
{
    public class CompanyConfiguration : BaseConfiguration<StCompany>
    {
        public override void Configure(EntityTypeBuilder<StCompany> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompanyCode });
        }
    }
}
