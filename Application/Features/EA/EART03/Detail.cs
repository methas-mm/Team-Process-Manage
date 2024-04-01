using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EART03
{
    public class Detail
    {
        public class Query : IRequest<EaCompetitionFormDTO>
        {
            public int Id { get; set; }
        }
        public class EaCompetitionFormDTO : EaCompetitionForm
        {
            public new IEnumerable<EaCompetitionGroupDTO> CompetitionGroups { get; set; }
        }
        public class EaCompetitionGroupDTO : EaCompetitionGroup
        {
            public new IEnumerable<EaCompetition> Competition { get; set; }
        }

        public class Handler : IRequestHandler<Query, EaCompetitionFormDTO>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<EaCompetitionFormDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql;
                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                f.competition_form_id as CompetitionFormId
                                ,f.competition_form_name_th as ""CompetitionFormNameTh""
                                ,f.competition_form_name_en as ""CompetitionFormNameEn""
                                ,f.competition_form_desc as ""CompetitionFormDesc""
                                ,f.active 
                                ,f.xmin as ""rowVersion""
                                from ea_competition_form f 
                                where f.competition_form_id = @CompetitionFormId ");
                var CompetitionForm = await _context.QueryFirstOrDefaultAsync<EaCompetitionFormDTO>(sql.ToString(), new { CompetitionFormId = request.Id }, cancellationToken);
                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                g.competition_group_id as ""CompetitionGroupId""
                                ,g.competition_form_id as ""CompetitionFormId""
                                ,g.competition_group_code as ""CompetitionGroupCode""
                                ,g.competition_group_th as ""CompetitionGroupTh""
                                ,g.competition_group_en as ""CompetitionGroupEn""
                                ,g.competition_group_desc as ""CompetitionGroupDesc""
                                ,g.kpi_point as ""KpiPoint""
                                ,g.active 
                                ,g.xmin as ""rowVersion""
                                from ea_competition_group g 
                                where g.competition_form_id = @CompetitionFormId order by g.competition_group_id");
                var competitionGroup = await _context.QueryAsync<EaCompetitionGroupDTO>(sql.ToString(), new { CompetitionFormId = request.Id }, cancellationToken);
                foreach (var item in competitionGroup)
                {
                    sql = new StringBuilder();
                    sql.AppendLine(@"select 
                        e.competition_id as ""competitionId""
                        ,e.competition_group_id as ""competitionGroupId""
                        ,e.competition_th as ""competitionTh""
                        ,e.competition_en as ""competitionEn""
                        ,e.competition_desc as ""competitionDesc""
                        ,e.kpi_point as ""kpiPoint""
                        ,e.active
                        ,e.xmin as ""rowVersion""
                        from ea_competition e
                        where 1=1 and e.competition_group_id=@GroupId order by competition_id ");
                    var x = await _context.QueryAsync<EaCompetition>(sql.ToString(), new { GroupId = item.CompetitionGroupId }, cancellationToken);
                    item.Competition = x;
                }
                CompetitionForm.CompetitionGroups = competitionGroup;
                return CompetitionForm;
            }
        }

    }
}
