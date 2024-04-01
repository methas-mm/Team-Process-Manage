using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.EA
{
    public class EaEvaluate : EntityBase
    {
        public int? EvaluateId { get; set; }
        public int? CompetitionFormId { get; set; }
        public int? CompetitionGroupId { get; set; }
        public int? CompetitionId { get; set; }
        public int Year { get; set; }
        public string status { get; set; }

        public int ActualPoinMid { get; set; }
        public int ActualPoinMidAnaly { get; set; }
        public int ActualPoinEnd { get; set; }
        public int ActualPoinEndAnaly { get; set; }

        public string RemarkMid { get; set; }
        public string RemarkEnd { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeCodeAnaly { get; set; }

        public ICollection<EaEvaluateDetail> EaEvaluateDetail { get; set; }
    }
}
