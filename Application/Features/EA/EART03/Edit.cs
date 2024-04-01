using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EART03
{
    public class Edit
    {
        public class Command : EaCompetitionForm, ICommand<int?>
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
                foreach (var item in request.CompetitionGroups)
                {
                    if (item.RowState == RowState.Add) 
                    {
                        item.CompetitionFormId = request.CompetitionFormId;
                        _context.Set<EaCompetitionGroup>().Add(item);
                    }
                    else if(item.RowState == RowState.Edit)
                    {
                        _context.Set<EaCompetitionGroup>().Attach(item);
                        _context.Entry(item).State = EntityState.Modified;
                    }
                    else if(item.RowState==RowState.Delete)
                    {
                        _context.Set<EaCompetitionGroup>().Remove(item);
                        var del =_context.Set<EaCompetition>().Where(m => m.CompetitionGroupId == item.CompetitionGroupId).ToList();
                        _context.Set<EaCompetition>().RemoveRange(del);
                    }
                }

                 _context.Set<EaCompetitionForm>().Attach((EaCompetitionForm)request);
                 _context.Entry((EaCompetitionForm)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                return request.CompetitionFormId;
            }
        }
    }
}
