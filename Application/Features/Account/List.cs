using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using System.Threading;
namespace Application.Features.Account
{
    public class List
    {
        public class ListWorKData
        {
            public IEnumerable<dynamic> ListTaskWork { get; set; }

            public IEnumerable<dynamic> ListBug { get; set; }
        }

        public class Query : IRequest<ListWorKData>
        {
            public class Handler : IRequestHandler<Query, ListWorKData>
            {
                private readonly ICleanDbContext _context;
                private readonly ICurrentUserAccessor _user;

                public Handler(ICleanDbContext context, ICurrentUserAccessor user)
                {
                    _context = context;
                    _user = user;
                }

                public async Task<ListWorKData> Handle(Query request, CancellationToken cancellationToken)
                {
                    ListWorKData listWorkData = new ListWorKData();
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine(@"select  pp.project_code  as projectcode 
                                , task_work_id as taskworkid 
                                , task_name  as taskname 
                                , estimate_md as estmd 
                                , get_wording_lang(@lang, pwg.workgroup_name_th , pwg.workgroup_name_en)  as workgroupname 
                                , get_wording_lang(@lang, pw.workcode_name_th , pw.workcode_name_en)  as workcodename 
                                , ptw.start_date as startdate , ptw.end_date  as endate
                                , get_wording_lang(@lang, ds.status_desc_th , ds.status_desc_en )  as statusname  , ds.status_color as statuscolor
                                from pm_task_work ptw 
                                inner join pm_project pp  on pp.project_id  = ptw.project_id 
                                inner join pm_workcode_group pwg on pwg.workcode_group_id  = ptw.workcode_group_id 
                                inner join pm_workcode pw  on pw.workcode_id  = ptw.workcode_id 
                                inner join db_status ds  on  ds.status_value  = ptw.status 
                                inner join st_user su on su.user_id  = @userid and su.employee_code  = ptw.employee_code_assign 
                                where ptw.status in ('N', 'A')
                                order by ptw.end_date ");

                    listWorkData.ListTaskWork = await _context.QueryAsync<dynamic>(sql.ToString(),
                                     new { lang = _user.Language , userid = _user.UserId}, cancellationToken);

                    sql = new StringBuilder();
                    sql.AppendLine(@"select 
                                    pp.project_code  as projectcode 
                                    , pmdp.module_detail_plan_code  as modulecode
                                    , pmdpp.program_code  as programcode
                                    , ptbs.task_bug_sub_id as bugsubid
                                    , task_bug_sub_name as bugname
                                    , due_date as duedate
                                    , get_wording_lang(@lang, dss.status_desc_th , dss.status_desc_en )  as statusname 
                                    , dss.status_color as statuscolor
                                    , get_wording_lang(@lang, dsp.status_desc_th , dsp.status_desc_en )  as priorityname 
                                    , dsp.status_color as prioritycolor
                                    from pm_task_bug_sub ptbs 
                                    inner join pm_task_bug ptb ON ptb.task_bug_id = ptbs.task_bug_id 
                                    inner join pm_project pp on pp.project_id  = ptb.project_id 
                                    inner join pm_module_detail_plan pmdp on pmdp.module_detail_plan_id  = ptb.module_detail_plan_id 
                                    inner join pm_module_detail_plan_program pmdpp on pmdpp.module_detail_plan_program_id = ptb.program_id 
                                    left join db_status dss on dss.status_value = ptbs.status and dss.column_name  = 'status'
                                    left join db_status dsp on dsp.status_value = ptbs.priority and dsp.column_name  = 'priority'
                                    inner join st_user su on su.user_id  = @userid and su.employee_code  = ptbs.employee_code_assign 
                                    where ptbs.status in ('N', 'A')
                                    order by ptbs.due_date  ");

                    listWorkData.ListBug = await _context.QueryAsync<dynamic>(sql.ToString(),
                                     new { lang = _user.Language, userid = _user.UserId }, cancellationToken);

                    return listWorkData;
                }

            }
        }
    }
}