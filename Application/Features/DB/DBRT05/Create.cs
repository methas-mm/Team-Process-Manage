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
using Domain.Entities.DB;
using MediatR;


namespace Application.Features.DB.DBRT05
{
    public class Create
    {
        public class Command : DbTeamSubEmployee, ICommand
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
                //if (_context.Set<DbTeamSubEmployee>().Any(i => i.teamId== request.teamId && i.employeeCode== request.employeeCode))
                //{
                //    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.PMRT10.CustomerCode");
                //}


                _context.Set<DbTeamSubEmployee>().Add((DbTeamSubEmployee)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}