using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbStatus : EntityBase
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string StatusValue { get; set; }
        public string StatusDescTh { get; set; }
        public string StatusDescEn { get; set; }
        public int Sequence { get; set; }
        public bool Active { get; set; }
    }
}
