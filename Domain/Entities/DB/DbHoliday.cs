using System;

namespace Domain.Entities.DB
{
    public class DbHoliday : EntityBase
    {
    
        public int? HolidayId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public DateTime? HolidayEndDate { get; set; }
        public string HolidayName { get; set; }
        public string HolidayDesc { get; set; }
        public bool Active { get; set; }
       
    }
}
