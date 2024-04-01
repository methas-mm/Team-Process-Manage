using Domain.Entities.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.CP
{
   public class CpCapacityDetail :  EntityBase
    {
        public int CapacityId { get; set; }
        public int? CapacityDetailId { get; set; }
        public string EmployeeCode { get; set; }
        public double? M01 { get; set; }
        public double? M02 { get; set; }
        public double? M03 { get; set; }
        public double? M04 { get; set; }
        public double? M05 { get; set; }
        public double? M06 { get; set; }
        public double? M07 { get; set; }
        public double? M08 { get; set; }
        public double? M09 { get; set; }
        public double? M10 { get; set; }
        public double? M11 { get; set; }
        public double? M12 { get; set; }

        public static explicit operator CpCapacityDetail(DbEmployee v)
        {
            throw new NotImplementedException();
        }
    }
}
