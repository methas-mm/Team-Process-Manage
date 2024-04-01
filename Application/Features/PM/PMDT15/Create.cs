using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class Create
    {
        public class Command : PmTaskWork, ICommand<long?>
        {

        }
        public class Handler : IRequestHandler<Command, long?>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<long?> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<PmTaskWork>().Add((PmTaskWork)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.TaskWorkId;
                
            }
        }
    }
}
