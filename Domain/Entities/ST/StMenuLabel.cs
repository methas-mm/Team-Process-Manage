using System;
using Domain.Types;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StMenuLabel : EntityBase
    {
        public string MenuName { get; set; }
        public Lang LangCode { get; set; }
        public string MenuCode { get; set; }
        public string SystemCode { get; set; }
        
        
        
    }
}
