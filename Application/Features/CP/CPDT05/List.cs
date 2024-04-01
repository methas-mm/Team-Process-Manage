using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CP.CPDT05
{
    public class List
    {

        public class Query : RequestPageQuery, IRequest<PageDto>
        {
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
                    sql.AppendLine(@"    select 
                                             cc.capacity_id as ""capacityId"",
                                             cc.year ,
                                             count(ccd.employee_code) as ""numberPeopleteam"", 
                                              ((sum(ccd.m01) + sum(ccd.m02) + sum(ccd.m03) + sum(ccd.m04 ) + sum(ccd.m05) + 
                                               sum(ccd.m06) + sum(ccd.m07) + sum(ccd.m08) + sum(ccd.m09) + sum(ccd.m10) + 
                                               sum(ccd.m11) + + sum(ccd.m12)) / count(ccd.employee_code) / 12 ) as  ""mdPeople"",
                                               sum(ccd.m01) + sum(ccd.m02) + sum(ccd.m03) + sum(ccd.m04 ) + sum(ccd.m05) + 
                                               sum(ccd.m06) + sum(ccd.m07) + sum(ccd.m08) + sum(ccd.m09) + sum(ccd.m10) + 
                                               sum(ccd.m11) + + sum(ccd.m12) as ""totalMD"" ,
                                               (select sum(ptw.actual_md) from pm_task_work ptw where extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and (extract(year from ptw.end_date)::text = (cc.year::int - 543)::text or ptw.end_date is null) and ptw.status = 'C')  as ""actuallyMD"",
                                               (select sum(ptw.estimate_md) from pm_task_work ptw where extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and (extract(year from ptw.end_date)::text = (cc.year::int - 543)::text or ptw.end_date is null) and ptw.status = 'C') as ""estimateMD"",
                                               (select sum(ptw.estimate_md) from pm_task_work ptw where extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and (extract(year from ptw.end_date)::text = (cc.year::int - 543)::text or ptw.end_date is null and ptw.status = 'C'))/
                                               (sum(ccd.m01) + sum(ccd.m02) + sum(ccd.m03) + sum(ccd.m04 ) + sum(ccd.m05) + 
                                               sum(ccd.m06) + sum(ccd.m07) + sum(ccd.m08) + sum(ccd.m09) + sum(ccd.m10) + 
                                               sum(ccd.m11) + + sum(ccd.m12)) * 100 as ""ratioPercen"",
                                             cc.xmin as ""rowVersion""
                                        from cp_capacity cc
                                        inner join cp_capacity_detail ccd  on ccd.capacity_id = cc.capacity_id
                                             inner join db_employee de on de.employee_code = ccd.employee_code 
                                             where de.team_id = (select de.team_id from db_employee de2 where de2.employee_code in (select su.employee_code from st_user su where user_id = @Userid))
                                             group by cc.year, cc.capacity_id
                                             order by cc.year asc" );

                    return await _context.GetPage(sql.ToString(), new { Language = this._user.Language, Userid = _user.UserId }, (RequestPageQuery)request, cancellationToken);

                }
            }
        }
    }
}

