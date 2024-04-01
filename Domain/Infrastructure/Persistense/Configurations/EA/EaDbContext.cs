using Application.Interfaces;
using Domain.Entities.EA;
using Microsoft.EntityFrameworkCore;
namespace Persistense
{
     public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<EaEvaluate> EaEvaluate { get; set; }
        public DbSet<EaCompetition> EaCompetition { get; set; }
        public DbSet<EaCompetitionForm> EaCompetitionForm { get; set; }
        public DbSet<EaCompetitionGroup> EaCompetitionGroup { get; set; }
    }
}
