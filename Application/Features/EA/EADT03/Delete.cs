using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class Delete
    {
        public class Command : ICommand
        {
            public int Id { get; set; }

            public uint RowVersion { get; set; }
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
                var user = new EaEvaluate { EvaluateId = request.Id, RowVersion = request.RowVersion };
                user.EaEvaluateDetail = await _context.Set<EaEvaluateDetail>().Where(o => o.EvaluateId == request.Id).ToListAsync(cancellationToken);
                _context.Set<EaEvaluate>().Attach(user);
                _context.Set<EaEvaluate>().Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
