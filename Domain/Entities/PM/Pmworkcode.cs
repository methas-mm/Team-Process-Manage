using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmWorkcode : EntityBase
    {
        public int? WorkcodeId { get; set; }
        public int? WorkcodeGroupId { get; set; }
        public string WorkcodeCode { get; set; }
        public string WorkcodeNameTh { get; set; }
        public string WorkcodeNameEn { get; set; }
        public string WorkcodeColor { get; set; }
        
    }
}
