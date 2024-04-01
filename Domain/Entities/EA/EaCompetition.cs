using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.EA
{
    public class EaCompetition : EntityBase
    {
        public int? CompetitionId { get; set; }
        public int CompetitionGroupId { get; set; }
        public string CompetitionTh { get; set; }
        public string CompetitionEn { get; set; }
        public string CompetitionDesc { get; set; }
        public int? KpiPoint { get; set; }
        public bool Active { get; set; }
        //public ICollection<EaEvaluate> EaEvaluate { get; set; }
    }
}
