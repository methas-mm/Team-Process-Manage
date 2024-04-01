using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbTeamId : EntityBase
    {
        public int? teamId { get; set; }
        public string companyCode { get; set; }
        public string teamNameTh { get; set; }
        public string teamNameEng { get; set; }
        public string employeeCode { get; set; }
    }
}
