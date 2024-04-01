using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbDistrict : EntityBase
    {
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictNameTha { get; set; }
        public string DistrictNameEng { get; set; }
        public bool? Active { get; set; }
    }
}
