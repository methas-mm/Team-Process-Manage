using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EART03
{
    public class Create
    {
        public class Command : EaCompetitionForm, ICommand<int?>
        {
            
        }
        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<EaCompetitionForm>().Any(i => i.CompetitionFormId == request.CompetitionFormId))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.EART03.Competition ");
                EaCompetitionForm Form = new EaCompetitionForm();
                Form.CompetitionFormId = request.CompetitionFormId;
                Form.CompetitionFormNameTh = request.CompetitionFormNameTh;
                Form.CompetitionFormNameEn = request.CompetitionFormNameEn;
                Form.CompetitionFormDesc = request.CompetitionFormDesc;
                Form.Active = request.Active;
                _context.Set<EaCompetitionForm>().Add((EaCompetitionForm)Form);
                await _context.SaveChangesAsync(cancellationToken);

                List<EaCompetitionGroup> Group = new List<EaCompetitionGroup>();
                foreach (var item in request.CompetitionGroups)
                {
                    item.CompetitionFormId = Form.CompetitionFormId;
                    Group.Add(item);
                }
                request.CompetitionGroups = Group;
                _context.Set<EaCompetitionGroup>().AddRange(Group);
                await _context.SaveChangesAsync(cancellationToken);
                return Form.CompetitionFormId;
            }
        }
    }
}
