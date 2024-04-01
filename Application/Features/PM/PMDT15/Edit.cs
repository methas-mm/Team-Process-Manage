using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class Edit
    {
        public class Command : PmTaskWork, ICommand<Unit>
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
                    _context.Set<PmTaskWork>().Attach((PmTaskWork)request);
                    _context.Entry((PmTaskWork)request).State = EntityState.Modified;
                    await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
