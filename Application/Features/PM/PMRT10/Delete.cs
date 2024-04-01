using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.PM;

namespace Application.Features.PM.PMRT10
{
    public class Delete
    {
        public class Command : PmCustomer, ICommand
        {
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = await _context.Set<PmCustomer>().FirstOrDefaultAsync(i => i.CustomerCode == request.CustomerCode);
                _context.Entry(item).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<PmCustomer>().Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}