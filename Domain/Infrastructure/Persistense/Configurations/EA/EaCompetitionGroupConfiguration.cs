using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.EA
{
    public class EaCompetitionGroupConfiguration : BaseConfiguration<EaCompetitionGroup>
    {
        public override void Configure(EntityTypeBuilder<EaCompetitionGroup> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompetitionGroupId });
        }
    }
}
