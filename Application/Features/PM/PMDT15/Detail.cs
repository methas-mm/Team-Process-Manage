using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class Detail
    {
        public class Query : RequestPageQuery, IRequest<PmTaskWork>
        {
            public long taskWorkId { get; set; }
        }
        public class PmTaskWorkDTO : PmTaskWork
        {
            public float FeatureEstimateMd { get; set; }
        }
        public class Handler : IRequestHandler<Query, PmTaskWork>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<PmTaskWork> Handle(Query request, CancellationToken cancellationToken)
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select ");
                sql.AppendLine("ptw.task_work_id as \"TaskWorkId\"");
                sql.AppendLine(",ptw.customer_id  as \"CustomerId\"");
                sql.AppendLine(",ptw.project_id as \"ProjectId\"");
                sql.AppendLine(",ptw.module_detail_plan_id as \"ModuleDetailPlanId\"");
                sql.AppendLine(",ptw.program_id as \"ProgramId\"");
                sql.AppendLine(",ptw.task_name as \"TaskName\"");
                sql.AppendLine(",ptw.estimate_md as \"EstimateMd\"");
                sql.AppendLine(",ptw.actual_md as \"ActualMd\"");
                sql.AppendLine(",ptw.start_date as \"StartDate\"");
                sql.AppendLine(",ptw.end_date as \"EndDate\"");
                sql.AppendLine(",ptw.employee_code_assign as \"EmployeeCodeAssign\"");
                sql.AppendLine(",ptw.status as \"Status\"");
                sql.AppendLine(",ptw.task_desc as \"TaskDesc\"");
                sql.AppendLine(",ptw.workcode_group_id as \"workcodeGroupId\"");
                sql.AppendLine(",ptw.workcode_id as \"workcodeId\"");
                sql.AppendLine(",case when pmdpp.estimate_md is not null then pmdpp.estimate_md else pmdp.estimate_md end as \"FeatureEstimateMd\"");
                sql.AppendLine(",ptw.xmin as \"rowVersion\"");
                sql.AppendLine("from pm_task_work ptw ");
                sql.AppendLine("left join pm_module_detail_plan_program pmdpp on pmdpp.module_detail_plan_program_id = ptw.program_id ");
                sql.AppendLine("left join pm_module_detail_plan pmdp on pmdp.module_detail_plan_id = ptw.module_detail_plan_id ");
                sql.AppendLine("where ptw.task_work_id = @id ");
                return await _context.QueryFirstOrDefaultAsync<PmTaskWorkDTO>(sql.ToString(), new
                {
                   id = request.taskWorkId
                }, cancellationToken);

            }
        }
    }
}
