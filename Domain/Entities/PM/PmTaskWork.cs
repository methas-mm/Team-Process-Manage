using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
   public class PmTaskWork : EntityBase
    {
        public long? TaskWorkId { get; set; }
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public int? ModuleDetailPlanId { get; set; }
        public int? ProgramId { get; set; }
        public string TaskName { get; set; }
        public float EstimateMd { get; set; }
        public float ActualMd { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EmployeeCodeAssign { get; set; }
        public string Status { get; set; }
        public string TaskDesc { get; set; }
        public long WorkcodeGroupId { get; set; }
        public long WorkcodeId { get; set; }
    }
}
