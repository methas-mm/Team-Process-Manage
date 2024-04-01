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

namespace Application.Features.PM.PMRT03
{
    public class Create
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
                if (_context.Set<PmWorkcodeGroup>().Any(o => o.WorkcodeGroupCode.ToUpper() == request.WorkcodeGroupCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.PMRT03.WorkcodeGroup");
                }

                _context.Set<PmWorkcodeGroup>().Add((PmWorkcodeGroup)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
