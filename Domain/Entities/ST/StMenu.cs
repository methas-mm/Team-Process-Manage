using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ST
{
    public class StMenu : EntityBase
    {
        public string MenuCode { get; set; }
        public string ProgramCode { get; set; }
        public string MainMenu { get; set; }
        public string SystemCode { get; set; }
        public string Icon { get; set; }
        public bool Active { get; set; }
        public ICollection<StMenu> SubMenus { get; set; }
        public ICollection<StMenuLabel> MenuLabels { get; set; }
    }
}
