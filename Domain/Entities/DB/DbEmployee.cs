using System;

namespace Domain.Entities.DB
{
    public class DbEmployee : EntityBase
    {
        public string CompanyCode { get; set; }
        public string EmployeeCode { get; set; }
        public string PersonalId { get; set; }
        public string PrefixId { get; set; }
        public string FirstNameTh { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameTh { get; set; }
        public string LastNameEn { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string PositionId { get; set; }
        public int? TeamId { get; set; }
        public DateTime? StartWorkDate { get; set; }
        public DateTime? EndWorkDate { get; set; }
        public string AddressNameTh { get; set; }
        public string AddressNameEn { get; set; }
        public int? SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string PostalCode { get; set; }
        public int? ImageId { get; set; }
        
    }
}
