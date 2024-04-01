using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.DB;

namespace Application.Features.DB.DBRT05
{
    public class Delete
    {
        public class Command : DbTeamSubEmployee, ICommand
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
                var item = await _context.Set<DbTeamSubEmployee>().FirstOrDefaultAsync(i => i.teamId == request.teamId);
                _context.Entry(item).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<DbTeamSubEmployee>().Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}