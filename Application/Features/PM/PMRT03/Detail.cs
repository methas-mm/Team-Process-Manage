using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.PM;
using Domain.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT03
{
    public class Detail
    {
        public class Query : IRequest<PmWorkcodeGroup>
        {
            public int? WorkcodeGroupId { get; set; }
            public string WorkcodeGroupCode { get; set; }
            public string? Type { get; set; }
            public Lang Language { get; set; }

        }
        public class Handler : IRequestHandler<Query, PmWorkcodeGroup>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<PmWorkcodeGroup> Handle(Query request, CancellationToken cancellationToken)
            {
               
                var program = await _context.Set<PmWorkcodeGroup>().Where(o => o.WorkcodeGroupCode == request.WorkcodeGroupCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (program == null)
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");

                //if (string.IsNullOrEmpty(request.Type))
                //{
                //    program.PmWorkcodes = await _context.Set<PmWorkcode>().Where(o => o.WorkcodeGroupId == request.WorkcodeGroupId).OrderBy(o => o.WorkcodeCode).AsNoTracking().ToListAsync(cancellationToken);
                //}

                return program;
            }
        }
    }
}
