using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.PM
{
    public class PmCustomer : EntityBase
    {
        public string CompanyCode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerNameTh { get; set; }
        public string CustomerNameEn { get; set; }
        public int? imageId { get; set; }
        public bool Active { get; set; }
    }
}
