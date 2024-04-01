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
    public class Delete
    {
        public class Command :  ICommand
        {
           
            public string Id { get; set; }
            public string Groupid { get; set; }
            public uint RowVersion { get; set; }


        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var item = await _context.Set<DbListItem>().FirstOrDefaultAsync(i => i.ListItemGroupCode == request.Id && i.ListItemCode == request.Groupid);
                _context.Entry(item).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<DbListItem>().Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }
    }
}
