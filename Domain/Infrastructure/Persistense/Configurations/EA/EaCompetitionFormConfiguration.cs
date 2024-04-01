using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.EA
{
    public class EaCompetitionFormConfiguration : BaseConfiguration<EaCompetitionForm>
    {
        public override void Configure(EntityTypeBuilder<EaCompetitionForm> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.CompetitionFormId });
        }
    }
}
