using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.DB
{
    public class HolidayConfiguration : BaseConfiguration<DbHoliday>
    {
        public override void Configure(EntityTypeBuilder<DbHoliday> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.HolidayId);
        }
    }
}
