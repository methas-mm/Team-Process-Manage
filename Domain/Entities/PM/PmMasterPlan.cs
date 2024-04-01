using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmMasterPlan : EntityBase
    {
        public int ProjectId { get; set; }
        public int? MasterPlanId { get; set; }
        public int WorkcodeGroupId { get; set; }
        public int WorkcodeId { get; set; }
        public string Status { get; set; }

        public ICollection<PmModuleDetailPlan> PmModuleDetailPlans { get; set; }
        public ICollection<PmMasterPlanAssign> PmMasterPlanAssign { get; set; }
    }
}
