using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT20
{
    public class Detail
    {
        public class Query : IRequest<PmTaskBug>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, PmTaskBug>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<PmTaskBug> Handle(Query request, CancellationToken cancellationToken)
            {
                PmTaskBug bug = await _context.Set<PmTaskBug>().Include(i => i.PmTaskBugSubs).Where(w => w.TaskBugId == request.Id).FirstOrDefaultAsync(cancellationToken);

                if (bug == null) throw new RestException(HttpStatusCode.NotFound, "message.STD00013");

                return bug;
            }
        }
    }
}
