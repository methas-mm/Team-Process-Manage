using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class Master
    {
        public class Query : IRequest<MasterData>
        {

        }
        public class MasterData
        {
            public IEnumerable<dynamic> Form { get; set; }
            public IEnumerable<dynamic> Empolyee { get; set; }
            public IEnumerable<dynamic> Status { get; set; }
        }

        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                MasterData Query = new MasterData();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select 
                                    f.competition_form_id as ""value""
                                    ,case @lang when 'th' then f.competition_form_name_th else f.competition_form_name_en end as ""text""
                                    from ea_competition_form f
                                    WHERE 1=1 and f.active = true");
                Query.Form = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                    case @lang when 'th' then 
                                    concat(e.employee_code , ' ',e.first_name_th,' ',e.last_name_th) 
                                    else concat(e.employee_code , ' ',e.first_name_en ,' ',e.last_name_en) end as ""text""
                                    ,e.employee_code as ""value""
                                    from db_employee e
                                    WHERE 1=1 order by e.employee_code");
                Query.Empolyee = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                s.status_value as ""value""
                                ,get_wording_lang(@lang,s.status_desc_th,s.status_desc_en) as ""text""
                                from db_status s where 1=1 and table_name ='ea_evaluate' and column_name ='evaluate'");
                Query.Status = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                return Query;
            }
        }
    }
}
