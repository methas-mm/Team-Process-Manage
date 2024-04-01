using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class Edit
    {
        public class Command : MasterPlanAssignDetail, ICommand<int>
        {

        }
        public class MasterPlanAssignDetail:EntityBase
        {
            public string WorkGroup { get; set; }
            public string WorkCode { get; set; }
            public int MasterPlanYear { get; set; }
            public List<MasterPlanAssignDetailPhase> Phrase { get; set; }
        }
        public class MasterPlanAssignDetailPhase :EntityBase
        {
            public string EmployeeCodeAssign { get; set; }
            public float EstimateMd { get; set; }
            public string Phrase { get; set; }
        }
        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                //_context.Set<PmTaskBugSub>().RemoveRange(request.PmTaskBugSubs.Where(o => o.RowState == RowState.Delete));
                //await _context.SaveChangesAsync(cancellationToken);

                //request.PmTaskBugSubs = request.PmTaskBugSubs.Where(o => o.RowState != RowState.Delete).ToList();

                //foreach (PmTaskBugSub bugSub in request.PmTaskBugSubs.Where(o => o.RowState == RowState.Edit))
                //{
                //    _context.Set<PmTaskBugSub>().Attach(bugSub);
                //    _context.Entry(bugSub).State = EntityState.Modified;
                //}

                //_context.Set<PmTaskBug>().Attach((PmTaskBug)request);
                //_context.Entry((PmTaskBug)request).State = EntityState.Modified;
                //await _context.SaveChangesAsync(cancellationToken);
                //return request.TaskBugId;
                return 1;
            }
        }
    }
}
