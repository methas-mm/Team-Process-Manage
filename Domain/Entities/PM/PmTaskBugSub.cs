using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmTaskBugSub : EntityBase
    {
        public int TaskBugSubId { get; set; }
        public int TaskBugId { get; set; }
        public string Status { get; set; }
        public string TaskBugSubName { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string EmployeeCodeAssign { get; set; }
        public string EmployeeCodeDelegate { get; set; }
        public string Desc { get; set; }
    }
}
