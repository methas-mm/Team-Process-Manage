using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.EA
{
    public class EaCompetitionGroup : EntityBase
    {
        public int? CompetitionGroupId { get; set; }
        public int? CompetitionFormId { get; set; }
        public string CompetitionGroupCode { get; set; }
        public string CompetitionGroupTh { get; set; }
        public string CompetitionGroupEn { get; set; }
        public string CompetitionGroupDesc { get; set; }
        public int? KpiPoint { get; set; }
        public bool Active { get; set; }
        //public ICollection<EaCompetition> EaCompetition { get; set; }
        //public ICollection<EaEvaluate> EaEvaluate { get; set; }
    }
}
