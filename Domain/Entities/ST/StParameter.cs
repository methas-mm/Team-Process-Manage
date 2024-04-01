using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StParameter : EntityBase
    {
        public string ParameterGroupCode { get; set; }
        public string ParameterCode { get; set; }
        public string ParameterValue { get; set; }
        public string Remark { get; set; }
    }
}
