using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.DB
{
    public class DbTeamSubEmployeeConfiguration : BaseConfiguration<DbTeamSubEmployee>
    {
        public override void Configure(EntityTypeBuilder<DbTeamSubEmployee> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.teamSubEmployeeId);
        }
    }
}
