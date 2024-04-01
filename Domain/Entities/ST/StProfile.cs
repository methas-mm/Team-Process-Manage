using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StProfile : EntityBase
    {
        public string ProfileCode { get; set; }
        public string ProfileDesc { get; set; }
        public bool Active { get; set; }
        public string SystemCode { get; set; }
        public ICollection<StMenuProfile> MenuProfiles { get; set; }
    }
}
