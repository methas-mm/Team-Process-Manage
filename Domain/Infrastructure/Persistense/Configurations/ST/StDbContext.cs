using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Domain.Entities.ST;
using Microsoft.EntityFrameworkCore;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<StMenu> StMenus { get; set; }
        public DbSet<StMenuLabel> StMenuLabels { get; set; }
        public DbSet<StMenuProfile> StMenuProfiles { get; set; }
        public DbSet<StProfile> StProfiles { get; set; }
        public DbSet<StParameter> StParameters { get; set; }
        public DbSet<StCompany> Companies { get; set; }
        public DbSet<StUser> StUsers { get; set; }
        public DbSet<StUserProfile> StUserProfiles { get; set; }
    }
}
