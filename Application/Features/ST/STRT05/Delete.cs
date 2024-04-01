using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using System.Linq;
using Domain.Entities.ST;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Features.ST.STRT05
{
    public class Delete
    {
        public class Command : ICommand
        {
            public long Id { get; set; }

            public uint RowVersion { get; set; }
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
                var user = new StUser { Id = request.Id, RowVersion = request.RowVersion };
                user.Profiles = await _context.Set<StUserProfile>().Where(o => o.Id == request.Id).ToListAsync(cancellationToken);
                _context.Set<StUser>().Attach(user);
                _context.Set<StUser>().Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

    }
}
