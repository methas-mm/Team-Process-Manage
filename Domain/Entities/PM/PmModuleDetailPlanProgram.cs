using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmModuleDetailPlanProgram : EntityBase
    {
        public int ModuleDetailPlanProgramId { get; set; }
        public int ModuleDetailPlanId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramNameTh { get; set; }
        public string ProgramNameEn { get; set; }
        public string ScopeType { get; set; }
        public float? EstimateMd { get; set; }
        public string Status { get; set; }
        public ICollection<PmModuleDetailPlanProgramAssign> ModuleDetailPlanProgramAssigns { get; set; }
        public PmModuleDetailPlan PmModuleDetailPlan { get; set; }
    }
}
