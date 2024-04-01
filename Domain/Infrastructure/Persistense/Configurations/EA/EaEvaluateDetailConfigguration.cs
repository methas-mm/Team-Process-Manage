using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.EA
{
    public class EaEvaluateDetailConfigguration: BaseConfiguration<EaEvaluateDetail>
    {
        public override void Configure(EntityTypeBuilder<EaEvaluateDetail> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.EvaluateDetailId });
        }
    }
}
