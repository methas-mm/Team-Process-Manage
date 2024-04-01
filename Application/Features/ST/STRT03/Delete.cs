using Domain.Entities.ST;
using Application.Behaviors;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Features.ST.STRT03
{
    public class Delete
    {
        public class Command : StProfile, ICommand
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
                var profile = await _context.Set<StProfile>().Include(i => i.MenuProfiles).FirstOrDefaultAsync(i => i.ProfileCode == request.ProfileCode);
                _context.Entry(profile).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<StProfile>().Remove(profile);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
