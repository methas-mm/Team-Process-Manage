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
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
        }
        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select 
                                e.evaluate_id as ""id""
                                ,e.year as ""year""
                                , (e.actual_poin_mid + e.actual_poin_mid_analy) as ""poinMid""
                                , (e.actual_poin_end + e.actual_poin_mid_analy) as ""poinEnd""
                                , get_wording_lang(@lang,s.status_desc_th,s.status_desc_en) as ""status""
                                ,case @lang when 'th' then
                                concat(emp.employee_code, ' ', emp.first_name_th, ' ', emp.last_name_th)
                                else concat(emp.employee_code, ' ', emp.first_name_en, ' ', emp.last_name_en) end as ""empName""
                                ,e.xmin as ""rowVersion""
                                from ea_evaluate e 
                                left join db_employee as emp on e.employee_code = emp.employee_code 
                                left join db_status s on e.status = s.status_value and s.column_name = 'evaluate' and s.table_name ='ea_evaluate'
                                where 1 = 1 ");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine($@"AND 
                                case @lang when 'th' then
                                concat(emp.employee_code, ' ', emp.first_name_th, ' ', emp.last_name_th)
                                else concat(emp.employee_code, ' ', emp.first_name_en, ' ', emp.last_name_en) end
                                like '%{request.Keyword}%'");
                }
                sql.AppendLine(@" ORDER BY e.evaluate_id ");
                return await _context.GetPage(sql.ToString(), new { lang = this._user.Language, keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }

        }

    }
}
