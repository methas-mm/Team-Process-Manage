using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Application.Behaviors;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT05
{
    public class Edit
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
                _context.Set<DbTeamSubEmployee>().Attach((DbTeamSubEmployee)request);
                _context.Entry((DbTeamSubEmployee)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}