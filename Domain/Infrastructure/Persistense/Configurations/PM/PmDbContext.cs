using Application.Interfaces;
using Domain.Entities.PM;
using Microsoft.EntityFrameworkCore;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<PmWorkcodeGroup> PmWorkcodeGroups { get; set; }
        public DbSet<PmWorkcode> PmWorkcodes { get; set; }
        public DbSet<PmTaskBug> PmTaskBugs { get; set; }
        public DbSet<PmTaskBugSub> PmTaskBugSubs { get; set; }
        public DbSet<PmCustomer> PmCustomers { get; set; }
        public DbSet<PmProject> PmProjects { get; set; }
        public DbSet<PmMasterPlan> PmMasterPlans { get; set; }
        public DbSet<PmModuleDetailPlan> PmModuleDetailPlans { get; set; }
        public DbSet<PmModuleDetailPlanProgram> PmModuleDetailPlanPrograms { get; set; }
        public DbSet<PmModuleDetailPlanProgramAssign> PmModuleDetailPlanProgramAssigns { get; set; }
        public DbSet<PmTaskWork> PmTaskWorks { get; set; }
        public DbSet<PmMasterPlanAssing> PmMasterPlanAssing { get; set; }
    }
}