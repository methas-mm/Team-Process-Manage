using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT10
{
   public class Edit
    {
        public class Command : DbEmployee, ICommand<DbEmployee>
        {

        }

        public class Handler : IRequestHandler<Command, DbEmployee>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<DbEmployee> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbEmployee>().Attach((DbEmployee)request);
                _context.Entry((DbEmployee)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return request;
            }
        }
    }
}
