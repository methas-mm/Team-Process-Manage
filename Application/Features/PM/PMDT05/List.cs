using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<IEnumerable<ProjectList>>
        {

        }
        public class ProjectList
        {
            public int projectId { get; set; }
            public string projectName { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public string imagePath { get; set; }
            public EmployeeList employeeList { get; set; }
        }
        public class EmployeeList
        {
            public string pm { get; set; }
            public string sa { get; set; }
            public string asa { get; set; }
            public string ssd { get; set; }
            public string sd { get; set; }
            public string module { get; set; }
            public string estimateMD { get; set; }
            public string actualMD { get; set; }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<ProjectList>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<IEnumerable<ProjectList>> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<ProjectList> projectLists = new List<ProjectList>();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select 
	                                pp.project_id ""projectId"",
                                    get_wording_lang(@lang,pp.project_name_th,pp.project_name_en) ""projectName"",
                                    pp.project_start_date ""startDate"",
                                    pp.project_end_date ""endDate""
                                from pm_project pp 
                                left join pm_master_plan pmp on pp.project_id = pmp.project_id 
                                left join pm_master_plan_assign pmpa on pmp.master_plan_id = pmpa.master_plan_id 
                                left join db_employee de on de.employee_code = pmpa.employee_code_assign 
                                group by ""projectId"",""projectName"",""startDate"",""endDate""
                                order by pp.project_start_date desc");
                projectLists = await _context.QueryAsync<ProjectList>(sql.ToString(), new { lang = _user.Language }, cancellationToken);

                foreach (ProjectList projectList in projectLists)
                {
                    sql = new StringBuilder();
                    sql.AppendLine(@"select 
    	                                        count(de.employee_code) filter(where de.position_id = 'PM') as pm,
                                                count(de.employee_code) filter(where de.position_id = 'SA') as sa,
                                                count(de.employee_code) filter(where de.position_id = 'ASA') as asa,
                                                count(de.employee_code) filter(where de.position_id = 'SSD') as ssd,
                                                count(de.employee_code) filter(where de.position_id = 'SD') as sd,
                                                count(pmdp.master_plan_id) as module,
                                                case when sum(pmpa.estimate_md) is null then 0 else sum(pmpa.estimate_md) end as estimateMD,
                                                0 as actualMD
                                    from pm_project pp 
                                    inner join pm_master_plan pmp on pp.project_id = pmp.project_id 
                                    left join pm_master_plan_assign pmpa on pmp.master_plan_id = pmpa.master_plan_id 
                                    left join pm_module_detail_plan pmdp on pmdp.master_plan_id = pmpa.master_plan_id 
                                    left join db_employee de on de.employee_code = pmpa.employee_code_assign 
                                    where pp.project_id = @projectId");
                    projectList.employeeList = await _context.QueryFirstAsync<EmployeeList>(sql.ToString(), new { projectId = projectList.projectId }, cancellationToken);
                }
                return projectLists;
            }
        }
    }
}
