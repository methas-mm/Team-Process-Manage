using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ST;

namespace Application.Features.ST.STRT01
{
    public class Detail
    {
        public class Query : IRequest<StCompany>
        {
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, StCompany>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<StCompany> Handle(Query request, CancellationToken cancellationToken)
            {
                StCompany data = await _context.Set<StCompany>().Where(c => c.CompanyCode == request.CompanyCode).FirstOrDefaultAsync(cancellationToken);
                return data;
            }


        }
    }
}
