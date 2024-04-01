using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Features.DB.DBRT03
{
    public class Class1 : EntityBase
    {
        [Key]
        public string Parametergroupcode { get; set; }
        public string Parametercode { get; set; }
        public string Parametervalue { get; set; }
        public string Remark { get; set; }
    }
}

