using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StUserProfile : EntityBase
    {
        public long Id { get; set; }
        public string ProfileCode { get; set; }
    }
}
