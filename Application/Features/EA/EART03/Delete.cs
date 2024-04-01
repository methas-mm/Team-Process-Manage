using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EART03
{
    public class Delete
    {
        public class Command : EaCompetitionForm, ICommand
        {
            public int Id { get; set; }
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
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"delete from ea_competition where 1=1 ");
                sql.AppendLine($@"AND competition_group_id IN(select g.competition_group_id from ea_competition_group g where 1=1 and competition_form_id={request.Id})");
                await _context.QueryAsync<EaCompetition>(sql.ToString(), new { lang = this._user.Language, keyword = request.Id }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine($@"delete from ea_competition_group g where 1=1 and g.competition_form_id ={request.Id}");
                await _context.QueryAsync<EaCompetitionGroup>(sql.ToString(), new { lang = this._user.Language, keyword = request.Id }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine($@"delete from ea_competition_form f where 1=1 and f.competition_form_id ={request.Id}");
                await _context.QueryAsync<EaCompetitionForm>(sql.ToString(), new { lang = this._user.Language, keyword = request.Id }, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
