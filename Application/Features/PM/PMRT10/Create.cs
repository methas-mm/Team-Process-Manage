using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;

namespace Application.Features.PM.PMRT10
{
    public class Create
    {
        public class Command : PmCustomer, ICommand
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
                if (_context.Set<PmCustomer>().Any(i => i.CustomerCode.ToUpper() == request.CustomerCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.PMRT10.CustomerCode");
                }

                request.CompanyCode = _user.Company;
                _context.Set<PmCustomer>().Add((PmCustomer)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}