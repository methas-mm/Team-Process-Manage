using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmWorkcodeGroup : EntityBase
    {
        public int? WorkcodeGroupId { get; set; }
        public string WorkcodeGroupCode { get; set; }
        public string WorkgroupNameTh { get; set; }
        public string WorkgroupNameEn { get; set; }
        public bool? Active { get; set; }
    }
}
