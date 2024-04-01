using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbTeamSubEmployee : EntityBase
    {
        public int? teamSubEmployeeId { get; set; }
        public int teamId { get; set; }
        public string employeeCode { get; set; }
    }
}
