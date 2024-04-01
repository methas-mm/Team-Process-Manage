
using Domain.Entities.ST;
using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Application.Exceptions;
using System.Net;

namespace Application.Features.ST.STRT01
{
   public class Create
   {
        public class Command : StCompany, ICommand<StCompany>
        {
           
        }
        public class Handler : IRequestHandler<Command, StCompany>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;

            }

            public async Task<StCompany> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<StCompany>().Any(i => i.CompanyCode == request.CompanyCode))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.STRT01.CompanyCode");

                _context.Set<StCompany>().Add((StCompany)request);
                await _context.SaveChangesAsync(cancellationToken);

                return request;
            }

           
        }
    }
}
