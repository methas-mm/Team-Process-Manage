
using Domain.Entities.ST;
using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT01
{
   public class Delete
    {
        public class Command : StCompany, ICommand
        {

        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var item = await _context.Set<StCompany>().FirstOrDefaultAsync(i => i.CompanyCode == request.CompanyCode);
                _context.Entry(item).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<StCompany>().Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
