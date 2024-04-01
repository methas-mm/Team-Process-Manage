using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.CP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CP.CPDT05
{
    public class Edit
    {
        public class Command : List<CpCapacityDetail>, ICommand<int?>
        {
            public int? CapacityId { get; set; }
        }

        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (CpCapacityDetail item in request)
                {
                    _context.Set<CpCapacityDetail>().Attach(item);
                    _context.Entry(item).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return request[0].CapacityId;
            }
        }
    }
}