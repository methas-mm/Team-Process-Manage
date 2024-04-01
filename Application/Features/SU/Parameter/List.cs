using Application.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Parameter
{
    public class List
    {
        public class Query : IRequest<IEnumerable<SuParameter>>
        {
            public string Group { get; set; }

        }
        public class Handler : IRequestHandler<Query, IEnumerable<SuParameter>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<SuParameter>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Set<SuParameter>().Where(o=>o.ParameterGroupCode == request.Group).AsNoTracking().ToListAsync(cancellationToken);
            }
        }
    }
}
