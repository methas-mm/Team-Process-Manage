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

namespace Application.Features.EA.EART03
{
    public class CUDCompetition
    {
        public class Command : List<EaCompetition>, ICommand<int?>
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
                foreach (var item in request)
                {
                    if (item.RowState == RowState.Delete)
                    {
                        _context.Set<EaCompetition>().Remove(item);
                    }
                    else if (item.RowState == RowState.Add)
                    {
                        _context.Set<EaCompetition>().Add(item);
                    } else if (item.RowState == RowState.Edit)
                    {
                        _context.Set<EaCompetition>().Attach(item);
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }
        }
    }
}
