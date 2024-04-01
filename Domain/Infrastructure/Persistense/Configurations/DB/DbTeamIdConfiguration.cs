using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.DB
{
    public class DbTeamIdConfiguration : BaseConfiguration<DbTeamId>
    {
        public override void Configure(EntityTypeBuilder<DbTeamId> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.teamId);
        }
    }
}
