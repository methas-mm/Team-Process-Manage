using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistense.Configurations.EA
{
    public class EaEvaluateConfiguration : BaseConfiguration<EaEvaluate>
    {
        public override void Configure(EntityTypeBuilder<EaEvaluate> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.EvaluateId });
            builder.HasMany(e => e.EaEvaluateDetail).WithOne().HasForeignKey(f => f.EvaluateId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}