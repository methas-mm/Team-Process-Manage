using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.CP
{
   public class CpCapacity : EntityBase
    {

        public int? CapacityId { get; set; }
        public string Year { get; set; }
        public ICollection<CpCapacityDetail> CpCapacityDetail { get; set; }
    }
}
