using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense.Configurations.DB
{
    public class DbStatusConfiguration : BaseConfiguration<DbStatus>
    {
        public override void Configure(EntityTypeBuilder<DbStatus> builder)
        {
            base.Configure(builder);
            builder.HasKey(p => new { p.TableName,p.ColumnName,p.Sequence });
        }
    }
}
