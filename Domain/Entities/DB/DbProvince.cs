using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbProvince : EntityBase
    {
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceNameTha { get; set; }
        public string ProvinceNameEng { get; set; }
        public string ProvinceShortNameTha { get; set; }
        public string ProvinceShortNameEng { get; set; }
        public bool Active { get; set; }
    }
}
