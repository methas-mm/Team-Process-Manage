using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.EA
{
    public class EaCompetitionConfiguration : BaseConfiguration<EaCompetition>
    {
        public override void Configure(EntityTypeBuilder<EaCompetition> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompetitionId });
        }
    }
}
