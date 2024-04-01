using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class Create
    {
        public class Command : PmMasterPlan, ICommand<int?>
        {

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
                _context.Set<PmMasterPlan>().Add(request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.MasterPlanId;

            }
        }
    }
}
