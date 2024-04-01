using Application.Interfaces;
using Domain.Entities.CP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CP.CPDT05
{
   public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Year { get; set; }
        }
        public class Query : IRequest<MasterData>
        {

        }
        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                var master = new MasterData();
                master.Year = GetYear();
                return master;
            }
            private IEnumerable<dynamic> GetYear()
            {
                var year = int.Parse(DateTime.Today.ToString("yyyy"));
                var yearsRange = Enumerable.Range(year - 10, 10).Concat(Enumerable.Range(year, 6));
                return yearsRange.Select(o => new { value = o, text = o }).OrderByDescending(y => y.value);
            }
        }

    }
}
