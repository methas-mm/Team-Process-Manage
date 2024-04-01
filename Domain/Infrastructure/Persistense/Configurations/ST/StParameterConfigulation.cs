using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.ST
{
    public class ParameterConfiguration : BaseConfiguration<StParameter>
    {
        public override void Configure(EntityTypeBuilder<StParameter> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.ParameterGroupCode, e.ParameterCode });
        }
    }
}
