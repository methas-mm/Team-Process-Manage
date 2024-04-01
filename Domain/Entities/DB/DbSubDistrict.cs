using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbSubDistrict : EntityBase
    {
        public int SubDistrictId { get; set; }
        public string SubDistrictNameTha { get; set; }
        public string SubDistrictNameEng { get; set; }
        public int DistrictId { get; set; }
        public bool Active { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictCodeMua { get; set; }
        public int ProvinceId { get; set; } 
    }

}
