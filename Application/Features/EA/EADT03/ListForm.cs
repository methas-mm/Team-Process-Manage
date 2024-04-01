using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class ListForm
    {
        public class Query : IRequest<IEnumerable<EaCompetitionGroupDTO>>
        {
            public int Id { get; set; }
            public int IdEvaluate { get; set; }
        }
        public class EaCompetitionGroupDTO
        {
            public int CompetitionGroupId { get; set; }
            public int CompetitionFormId { get; set; }
            public string CompetitionGroupName { get; set; }
            public int KpiPoint { get; set; }
            public new IEnumerable<EaCompetitionDTO> Competition { get; set; }
        }
        public class EaCompetitionDTO : EaCompetition
        {
            public string CompetitionName { get; set; }
            public int actualPoinMid { get; set; }
            public int actualPoinEnd { get; set; }
        }


        public class Handler : IRequestHandler<Query, IEnumerable<EaCompetitionGroupDTO>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<EaCompetitionGroupDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql;
                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                g.competition_group_id as ""CompetitionGroupId""
                                ,g.competition_form_id as ""CompetitionFormId""
                                ,case @lang when 'th' then g.competition_group_th else g.competition_group_en end AS ""CompetitionGroupName""
                                ,g.kpi_point as ""KpiPoint""
                                from ea_competition_group g 
                                where g.competition_form_id = @CompetitionFormId and g.active=true order by g.competition_group_id");
                var competitionGroup = await _context.QueryAsync<EaCompetitionGroupDTO>(sql.ToString(), new { lang = this._user.Language, CompetitionFormId = request.Id }, cancellationToken);
                foreach (var item in competitionGroup)
                {
                    sql = new StringBuilder();
                    sql.AppendLine(@"select 
                        e.competition_id as ""competitionId""
                        ,e.competition_group_id as ""competitionGroupId""
                        ,case @lang when 'th' then e.competition_th else e.competition_en end AS ""CompetitionName""
                        ,e.kpi_point as ""kpiPoint""
                        ,d.actual_poin_mid as ""actualPoinMid""
                        ,d.actual_poin_end as ""actualPoinEnd""
                        from ea_competition e
                        left join ea_evaluate_detail d 
                        on e.competition_group_id=d.competition_group_id and e.competition_id =d.competition_id and d.competition_form_id = @CompetitionFormId and d.evaluate_id =@IdEvaluate
                        where 1=1 and e.competition_group_id=@GroupId and e.active=true order by e.competition_id ");
                    var x = await _context.QueryAsync<EaCompetitionDTO>(sql.ToString(), new { lang = this._user.Language, GroupId = item.CompetitionGroupId, CompetitionFormId = request.Id, IdEvaluate = request.IdEvaluate }, cancellationToken);
                    item.Competition = x;
                }
                return competitionGroup;
            }
        }

    }
}
