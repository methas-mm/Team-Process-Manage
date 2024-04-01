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

namespace Application.Features.PM.PMRT03
{
    public class Edit
    {
        public class Command : PmWorkcodeGroup, ICommand
        {
            public string Table { get; set; }
            public PmWorkcodeGroup WorkcodeGroup { get; set; }
            //public IEnumerable <PmWorkcode> Workcodes { get; set; }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //if (request.Table.ToLower() == "workcodegroup")
                //{
                //    _context.Set<PmWorkcodeGroup>().Attach((PmWorkcodeGroup)request.WorkcodeGroup);
                //    _context.Entry((PmWorkcodeGroup)request.WorkcodeGroup).State = EntityState.Modified;
                //    await _context.SaveChangesAsync(cancellationToken);
                //}
                //else
                //{
                //    //Delete
                //    _context.Set<PmWorkcode>().RemoveRange(request.Workcodes.Where(o => o.RowState == RowState.Delete));
                //    await _context.SaveChangesAsync(cancellationToken);

                //    //Add
                //    _context.Set<PmWorkcode>().AddRange(request.Workcodes.Where(o => o.RowState == RowState.Add));
                //    await _context.SaveChangesAsync(cancellationToken);

                //    //Edit
                //    foreach (var workcodes in request.Workcodes.Where(o => o.RowState == RowState.Edit))
                //    {
                //        _context.Set<PmWorkcode>().Attach(workcodes);
                //        _context.Entry(workcodes).State = EntityState.Modified;
                //    }
                //    await _context.SaveChangesAsync(cancellationToken);
                //}

                return Unit.Value;
            }
        }
    }
}
