using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SU
{
    public class SuDivision : EntityBase
    {
        public string CompanyCode { get; set; }
        public string DivCode { get; set; }
        public string DivNameTha { get; set; }
        public string DivNameEng { get; set; }
        public string DivParent { get; set; }
        public ICollection<SuDivision> Divisions { get; set; }
    }
}
