using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities.DB
{
    public class DbPostalCode : EntityBase
    {
        public int PostalCodeId { get; set; }
        public string PostalCode { get; set; }
        public int SubDistrictId { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public string PostalNameTha { get; set; }
        public string PostalNameEng { get; set; }
        public string Remark { get; set; }
        public bool Active { get; set; }
    }
}
