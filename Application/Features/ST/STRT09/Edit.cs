using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT09
{
    public class Edit
    {
        public class Command : DbListItem, ICommand<DbListItem>
        {
            public class Handler : IRequestHandler<Command, DbListItem>
            {
                private readonly ICleanDbContext _context;
                public Handler(ICleanDbContext context)
                {
                    _context = context;
                }

                public async Task<DbListItem> Handle(Command request, CancellationToken cancellationToken)
                {
                    _context.Set<DbListItem>().Attach((DbListItem)request);
                    _context.Entry((DbListItem)request).State = EntityState.Modified;
                    await _context.SaveChangesAsync(cancellationToken);
                    return request;
                }
            }
        }
    }
}
