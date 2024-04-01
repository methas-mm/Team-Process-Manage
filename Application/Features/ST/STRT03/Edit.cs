using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.ST;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ST.STRT03
{
    public class Edit
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
                _context.Set<StMenuProfile>().RemoveRange(request.MenuProfiles.Where(i => i.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.MenuProfiles = request.MenuProfiles.Where(i => i.RowState != RowState.Delete).ToList();

                _context.Set<StProfile>().Attach((StProfile)request);
                _context.Entry((StProfile)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
