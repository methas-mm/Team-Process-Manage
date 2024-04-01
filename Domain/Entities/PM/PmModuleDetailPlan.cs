using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmModuleDetailPlan : EntityBase
    {
        public int ModuleDetailPlanId { get; set; }
        public int MasterPlanId { get; set; }
        public string ModuleDetailPlanCode { get; set; }
        public string ModuleDetailPlanNameTh { get; set; }
        public string ModuleDetailPlanNameEn { get; set; }
        public float? EstimateMd { get; set; }
        public string Status { get; set; }
        public string EmployeeCodeKeeper { get; set; }
        public ICollection<PmModuleDetailPlanProgram> ModuleDetailPlanPrograms { get; set; }
    }
}
