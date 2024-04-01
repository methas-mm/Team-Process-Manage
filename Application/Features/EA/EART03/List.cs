using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EART03
{
    public class List
    {
        public class Query : IRequest<IEnumerable<EaCompetitionForm>>
        {
            public string Keyword { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<EaCompetitionForm>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;

            }

            public async Task<IEnumerable<EaCompetitionForm>> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"SELECT
                        f.competition_form_id 
                        ,f.competition_form_name_th 
                        ,f.competition_form_name_en 
                        ,f.active
                        ,f.xmin AS ""rowVersion""
                        FROM ea_competition_form f WHERE 1=1 ");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine($@"AND CONCAT(f.competition_form_name_en,f.competition_form_name_th) like '%{request.Keyword}%'");
                }
                sql.AppendLine(@" ORDER BY f.competition_form_id ");
                IEnumerable<EaCompetitionForm> Query = new List<EaCompetitionForm>();
                Query = await _context.QueryAsync<EaCompetitionForm>(sql.ToString(), new { lang = this._user.Language, keyword = request.Keyword }, cancellationToken);
                return Query;
            }

        }
    }
}
