using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.CP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CP.CPDT05
{
    public class Delete
    {
        public class Command : CpCapacity, ICommand
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
                var item = await _context.Set<CpCapacity>().Include(x => x.CpCapacityDetail).FirstOrDefaultAsync(i => i.CapacityId == request.CapacityId);
                _context.Entry(item).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<CpCapacity>().Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
