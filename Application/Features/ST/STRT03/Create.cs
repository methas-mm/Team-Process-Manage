using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT03
{
    public class Create
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
                if (_context.Set<StProfile>().Any(i => i.ProfileCode.ToUpper() == request.ProfileCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.STRT03.ProfileCode");
                }
                _context.Set<StProfile>().Add((StProfile)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
                
            }
        }
    }
}
