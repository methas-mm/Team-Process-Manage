using Application.Interfaces;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<DbHoliday> DbHolidays { get; set; }
        public DbSet<DbPostalCode> DbPostalCodes { get; set; }
        public DbSet<DbProvince> DbProvinces { get; set; }
        public DbSet<DbDistrict> DbDistricts { get; set; }
        public DbSet<DbSubDistrict> DbSubDistricts { get; set; }
        public DbSet<DbDistrict> DbDistrict { get; set; }
        public DbSet<DbEmployee> Employees { get; set; }
        public DbSet<DbPostalCode> DbPostalCode { get; set; }
        public DbSet<DbProvince> DbProvince { get; set; }
        public DbSet<DbStatus> DbStatuses { get; set; }
        public DbSet<DbListItem> DbListItem { get; set; }
        public DbSet<DbTeamSubEmployee> DbTeamSubEmployee { get; set; }
    }
}
