using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class Delete
    {
        public class Command : PmTaskWork, ICommand
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
                _context.Entry(request).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<PmTaskWork>().Remove(request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
