using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public abstract class EntityBaseOracle
    {
        public string CrBy { get; set; }
        public DateTime? CrDate { get; set; }
        public string ProgId { get; set; }
        public string UpdBy { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
