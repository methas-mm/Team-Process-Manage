using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmTaskBug : EntityBase
    {
        public int TaskBugId { get; set; }
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public int ModuleDetailPlanId { get; set; }
        public int ProgramId { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public ICollection<PmTaskBugSub> PmTaskBugSubs { get; set; }
    }
}
