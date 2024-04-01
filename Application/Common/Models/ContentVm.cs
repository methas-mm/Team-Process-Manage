using Domain.Entities.SU;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class ContentVm
    {
        public SuContent Content { get; set; }
        public string DisplayPath { get; set; }
        public string LocalPath { get; set; }
    }
}
