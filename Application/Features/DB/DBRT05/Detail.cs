using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT05
{
    public class Detail
    {
        public class Query : IRequest<DbTeamSubEmployee>
        {
            public int? TeamId { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbTeamSubEmployee>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<DbTeamSubEmployee> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _context.Set<DbTeamSubEmployee>().Where(c => c.teamId == request.TeamId).FirstOrDefaultAsync(cancellationToken);
                return data;
            }
        }
    }
}
