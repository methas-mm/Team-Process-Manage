using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StCompany : EntityBase
    {
        public string CompanyCode { get; set; }
        public string CompanyNameTh { get; set; }
        public string CompanyNameEng { get; set; }
        public string AddressNameTh { get; set; }
        public string AddressNameEng { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubDistrictId { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public int? ImageId { get; set; }
        public bool Active { get; set; }
   
    }
}
