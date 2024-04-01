using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.ST;
using System.Linq;
using Application.Exceptions;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ST.STRT03
{
    public class Copy
    {

        public class Command : ICommand
        {
            public string ProfileCodeFrom { get; set; }
            public string ProfileDescFrom { get; set; }
            public string ProfileCodeTo { get; set; }
            public string ProfileDescTo { get; set; }
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
                if (_context.Set<StProfile>().Any(i => i.ProfileCode.ToUpper() == request.ProfileCodeTo.ToUpper()))
                    throw new RestException(HttpStatusCode.BadRequest, "message.ST00001");

                StProfile stProfile = new StProfile();
                stProfile.ProfileCode = request.ProfileCodeTo;
                stProfile.ProfileDesc = request.ProfileDescTo;
                stProfile.Active = true;
                _context.Set<StProfile>().Add((StProfile)stProfile);
                await _context.SaveChangesAsync(cancellationToken);

                List<StMenuProfile> stMenuProfiles = await _context.Set<StMenuProfile>().Where(e => e.ProfileCode == request.ProfileCodeFrom).AsNoTracking().ToListAsync(cancellationToken);
                foreach (StMenuProfile stMenuProfile in stMenuProfiles)
                    stMenuProfile.ProfileCode = request.ProfileCodeTo;

                _context.Set<StMenuProfile>().AddRange(stMenuProfiles);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
