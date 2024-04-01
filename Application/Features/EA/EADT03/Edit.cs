using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class Edit
    {
        public class Command : EaEvaluate, ICommand<int?>
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
                foreach (var item in request.EaEvaluateDetail)
                {
                    var x = await _context.Set<EaEvaluateDetail>().Where(
                        e => e.EvaluateId == item.EvaluateId &&
                        e.CompetitionFormId == item.CompetitionFormId &&
                        e.CompetitionGroupId == item.CompetitionGroupId &&
                        e.CompetitionId == item.CompetitionId
                        ).FirstOrDefaultAsync(cancellationToken);
                    if (x.ActualPoinEnd != item.ActualPoinEnd || x.ActualPoinMid != item.ActualPoinMid)
                    {
                        x.ActualPoinEnd = item.ActualPoinEnd;
                        x.ActualPoinMid = item.ActualPoinMid;
                        x.RowState = RowState.Normal;
                        _context.Set<EaEvaluateDetail>().Attach(x);
                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }

                EaEvaluate evaluate = new EaEvaluate();
                evaluate = await _context.Set<EaEvaluate>().Where(e=>e.EvaluateId==request.EvaluateId).FirstOrDefaultAsync(cancellationToken);
                evaluate.ActualPoinEnd = request.ActualPoinEnd;
                evaluate.ActualPoinMid = request.ActualPoinMid;
                evaluate.status = request.status;
                _context.Set<EaEvaluate>().Attach(evaluate);
                _context.Entry(evaluate).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                return request.EvaluateId;
            }
        }
    }
}
