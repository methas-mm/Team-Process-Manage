using Application.Interfaces;
using Domain.Entities.CP;
using Microsoft.EntityFrameworkCore;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<CpCapacity> CpCpacity { get; set; }
        public DbSet<CpCapacityDetail> CpCapacityDetail { get; set; }

    }
}
