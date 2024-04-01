using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.EA
{
    public class EaCompetitionForm :EntityBase
    {
        public int? CompetitionFormId { get; set; }
        public string CompetitionFormNameTh { get; set; }
        public string CompetitionFormNameEn { get; set; }
        public string CompetitionFormDesc { get; set; }
        public bool Active { get; set; }
        public ICollection<EaCompetitionGroup> CompetitionGroups { get; set; }
        //public ICollection<EaEvaluate> EaEvaluate { get; set; }
    }
}
