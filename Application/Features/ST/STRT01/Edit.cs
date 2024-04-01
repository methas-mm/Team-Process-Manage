using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ST;

namespace Application.Features.ST.STRT01
{
   public class Edit
    {
        public class Command : StCompany, ICommand<StCompany>
        {

        }

        public class Handler : IRequestHandler<Command, StCompany>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<StCompany> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<StCompany>().Attach((StCompany)request);
                _context.Entry((StCompany)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return request;
            }
        }
    }
}
