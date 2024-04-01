using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT09
{
    public class Detail
    {
        public class Query : IRequest<DbListItem>
        {
            public string Id { get; set; }
            public string Groupid { get; set; }
        }
        public class Handler : IRequestHandler<Query, DbListItem>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<DbListItem> Handle(Query request, CancellationToken cancellationToken)
            {
                DbListItem data = await _context.Set<DbListItem>().Where(c => c.ListItemGroupCode == request.Id  && c.ListItemCode == request.Groupid).FirstOrDefaultAsync(cancellationToken);
                return data;
            }

           
        }
    }
}
