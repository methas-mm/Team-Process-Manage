using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Application.Features.PM.PMDT15
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Object>>
        {
            public int? ProjectId { get; set; }
            public string EmployeeCodeAssign { get; set; }

        }
        public class Handler : IRequestHandler<Query, IEnumerable<Object>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<IEnumerable<Object>> Handle(Query request, CancellationToken cancellationToken)
            {
                long UserId = _user.UserId;
                StringBuilder sql;
                sql = new StringBuilder();

                sql.AppendLine(@"select de.team_id as ""TeamId"" ,dt.employee_code as ""LeaderCode"",de.employee_code as ""EmployeeCode""
                                from db_employee de 
                                inner join st_user su on su.employee_code = de.employee_code 
                                 left join db_team dt ON de.team_id = dt.team_id 
                                where su.user_id = @id");
                var EmpData = await _context.QuerySingleOrDefaultAsync<dynamic>(sql.ToString(), new { id = UserId }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine("select ");
                sql.AppendLine("ptw.task_work_id as \"taskWorkId\"");
                sql.AppendLine(",concat(pc.customer_code ,' : ',ptw.task_name) as \"title\"");
                sql.AppendLine(",ptw.start_date as \"start\"");
                sql.AppendLine(",ptw.end_date as \"end\"");
                sql.AppendLine($",case when  { EmpData.LeaderCode == EmpData.EmployeeCode}  then true else false end as \"leaderRole\"");
                sql.AppendLine("from pm_task_work ptw ");
                sql.AppendLine("inner join pm_workcode_group pwg ON  pwg.workcode_group_id = ptw.workcode_group_id ");
                sql.AppendLine("inner join pm_workcode pw on pw.workcode_id = ptw.workcode_id ");
                sql.AppendLine("inner join db_employee de on de.employee_code = ptw.employee_code_assign ");
                sql.AppendLine("left join pm_customer pc on pc.customer_id = ptw.customer_id ");
                sql.AppendLine("where 1 = 1");
                if (request.ProjectId != null) sql.AppendLine(@"and ptw.project_id  = @ProjectId");
                // Role Leader Team.
                if (EmpData != null && (EmpData.LeaderCode == EmpData.EmployeeCode))
                {
                    sql.AppendLine(@"and de.team_id = @TeamId");
                    if (request.EmployeeCodeAssign != null) sql.AppendLine(@"and ptw.employee_code_assign = @EmployeeCodeAssign");
                }
                // Role Crewmate
                else
                {
                    sql.AppendLine(@"and ptw.employee_code_assign = @EmployeeCodeAssign");
                }
                sql.AppendLine($@"union
                                select
                                  null as ""taskWorkId""
                                , case @lang when 'th' then concat('วันหยุด',' : ',dh.holiday_name) else concat('Holiday',' : ',dh.holiday_name) end as ""title""
                                , dh.holiday_date as ""start""
                                , dh.holiday_end_date as ""end""
                                ,case when  { EmpData.LeaderCode == EmpData.EmployeeCode}  then true else false end as ""leaderRole""
                                from db_holiday dh  where dh.active = true and holiday_date is not null");

                return await _context.QueryAsync<dynamic>(
                    sql.ToString(),
                    new
                    {
                        ProjectId = request.ProjectId,
                        EmployeeCodeAssign = (EmpData.LeaderCode == EmpData.EmployeeCode) ? request.EmployeeCodeAssign : EmpData.EmployeeCode,
                        TeamId = EmpData.TeamId ,
                        lang = this._user.Language
                    },
                    cancellationToken
                );
            }
        }
    }
}
