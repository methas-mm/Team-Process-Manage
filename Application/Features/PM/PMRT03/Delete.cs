using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT03
{
    public class Delete
    {
        public class Command : PmWorkcodeGroup, ICommand
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
                //var workcodeGroup = await _context.Set<PmWorkcodeGroup>().Include(i => i.PmWorkcodes).FirstOrDefaultAsync(i => i.WorkcodeGroupCode == request.WorkcodeGroupCode);
                //_context.Entry(workcodeGroup).Property("RowVersion").OriginalValue = request.RowVersion;
                //_context.Set<PmWorkcodeGroup>().Remove(workcodeGroup);
                //await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
