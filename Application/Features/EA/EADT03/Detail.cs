using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class Detail
    {
        public class Query : IRequest<EaEvaluate>
        {
            public int Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, EaEvaluate>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<EaEvaluate> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Set<EaEvaluate>().Where(i => i.EvaluateId == request.Id).FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}
