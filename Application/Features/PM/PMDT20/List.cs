using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT20
{
    public class List
    {
        public class Tree
        {
            public string Name { get; set; }
            public int id { get; set; }
            public int Value { get; set; }
            public string Desc { get; set; }
            public string Color { get; set; }
            public string Status { get; set; }
            public string StatusColor { get; set; }
            public string Priority { get; set; }
            public string PriorityColor { get; set; }
            public ICollection<Tree> Children { get; set; }
        }

        public class Query : IRequest<IEnumerable<Tree>>
        {
            public int? CustomerId { get; set; }
            public int? ProjectId { get; set; }
            public int? ModuleId { get; set; }
            public int? ProgramId { get; set; }
            public string Topic { get; set; }
            public string Status { get; set; }
            public string Priority { get; set; }
            public string Employee { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Tree>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<IEnumerable<Tree>> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<Tree> trees = new List<Tree>();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"
                    select distinct	concat(pp.project_code, ' : ', get_wording_lang(@lang , pp.project_name_th , pp.project_name_en)) ""name"",
          		                    pp.project_id ""id"",
            	                    pp.created_date ""desc"",
              	                    ds.status_color ""color"",
        		                    get_wording_lang(@lang , ds.status_desc_th , ds.status_desc_en) ""status"",
           		                    ds.status_color ""statusColor""
                    from			pm_customer pc
                    inner join		pm_project pp
                    on      		pp.customer_id = pc.customer_id
                    inner join		pm_master_plan pmp
                    on				pmp.project_id = pp.project_id
                    inner join		pm_module_detail_plan pmdp
                    on				pmdp.master_plan_id = pmp.master_plan_id
                    inner join		pm_module_detail_plan_program pmdpp
                    on				pmdpp.module_detail_plan_id = pmdp.module_detail_plan_id
                    inner join		pm_module_detail_plan_program_assign pmdppa
                    on				pmdppa.module_detail_plan_program_id = pmdpp.module_detail_plan_program_id
                    inner join		pm_task_bug ptb
                    on				ptb.program_id = pmdppa.module_detail_plan_program_id
                    inner join		pm_task_bug_sub ptbs
                    on				ptbs.task_bug_id = ptb.task_bug_id
                    left join 		db_status ds
                    on          	ds.status_value = ptb.status
            	                    and ds.table_name = 'pm_task_sub'
            	                    and ds.column_name = 'status'
                    left join   	db_status ds2
                    on          	ds2.status_value = ptbs.priority
            	                    and ds2.table_name = 'pm_task_sub'
            	                    and ds2.column_name = 'priority'
                    where     		1 = 1");

                if (request.CustomerId != null)
                    sql.AppendLine(@"and pc.customer_id = @customerId");

                if (request.ProjectId != null)
                    sql.AppendLine(@"and pp.project_id = @projectId");

                if (request.ModuleId != null)
                    sql.AppendLine(@"and pmdp.module_detail_plan_id = @moduleId");

                if (request.ProgramId != null)
                    sql.AppendLine(@"and pmdpp.module_detail_plan_program_id = @programId");

                if (!string.IsNullOrEmpty(request.Topic))
                    sql.AppendLine(@"and ptbs.task_bug_sub_name ilike concat('%', @topic, '%')");

                if (!string.IsNullOrEmpty(request.Status))
                    sql.AppendLine(@"and ptbs.status = @status");

                if (!string.IsNullOrEmpty(request.Priority))
                    sql.AppendLine(@"and ptbs.priority = @priority");

                if (!string.IsNullOrEmpty(request.Employee))
                    sql.AppendLine(@"and (ptbs.employee_code_assign = @employee or ptbs.employee_code_delegate = @employee)");

                trees = await _context.QueryAsync<Tree>(sql.ToString(), new
                {
                    lang = _user.Language,
                    customerId = request.CustomerId,
                    projectId = request.ProjectId,
                    programId = request.ProgramId,
                    moduleId = request.ModuleId,
                    topic = request.Topic,
                    status = request.Status,
                    priority = request.Priority,
                    employee = request.Employee
                }, cancellationToken);

                foreach (Tree tree in trees)
                {
                    sql = new StringBuilder();
                    sql.AppendLine(@"
                        select		concat(pmdpp.program_code, ' : ', get_wording_lang(@lang , pmdpp.program_name_th , pmdpp.program_name_en)) ""name"",
                                    pmdpp.module_detail_plan_program_id ""id"",
                                    ptb.task_bug_id ""value""
                        from        pm_task_bug ptb
                        inner join  pm_module_detail_plan_program pmdpp
                        on          pmdpp.module_detail_plan_program_id = ptb.program_id
                        where       ptb.project_id = @projectId");

                    if (request.ProgramId != null)
                        sql.AppendLine(@"and pmdpp.module_detail_plan_program_id = @programId");

                    IEnumerable<Tree> programs = await _context.QueryAsync<Tree>(sql.ToString(), new {
                        lang = _user.Language,
                        projectId = tree.id,
                        programId = request.ProgramId
                    }, cancellationToken);

                    foreach (Tree program in programs)
                    {
                        if (tree.Children == null) tree.Children = new List<Tree>();
                        tree.Children.Add(program);
                        sql = new StringBuilder();
                        sql.AppendLine(@"
                            select		concat(ptbs.task_bug_sub_id, ' : ', ptbs.task_bug_sub_name) ""name"",
                                        ptbs.task_bug_sub_id id,
                                        ptb.task_bug_id ""value"",
                                        get_wording_lang(@lang , ds.status_desc_th , ds.status_desc_en) status,
                                        ds.status_color ""statusColor"",
                                        get_wording_lang(@lang , ds2.status_desc_th , ds2.status_desc_en) priority,
                                        ds2.status_color ""priorityColor""
                            from		pm_task_bug ptb
                            inner join	pm_task_bug_sub ptbs
                            on			ptbs.task_bug_id = ptb.task_bug_id
                            left join  db_status ds
                            on          ds.status_value = ptbs.status
                                        and ds.table_name = 'pm_task_sub'
                                        and ds.column_name = 'status'
                            left join  db_status ds2
                            on          ds2.status_value = ptbs.priority
                                        and ds2.table_name = 'pm_task_sub'
                                        and ds2.column_name = 'priority'
                            where       ptb.program_id = @programId");

                        if (!string.IsNullOrEmpty(request.Topic))
                            sql.AppendLine(@"and ptbs.task_bug_sub_name ilike concat('%', @topic, '%')");

                        if (!string.IsNullOrEmpty(request.Status))
                            sql.AppendLine(@"and ptbs.status = @status");

                        if (!string.IsNullOrEmpty(request.Priority))
                            sql.AppendLine(@"and ptbs.priority = @priority");

                        if (!string.IsNullOrEmpty(request.Employee))
                            sql.AppendLine(@"and (ptbs.employee_code_assign = @employee or ptbs.employee_code_delegate = @employee)");
                        
                        IEnumerable<Tree> bugs = await _context.QueryAsync<Tree>(sql.ToString(), new {
                            lang = _user.Language,
                            programId = program.id,
                            topic = request.Topic,
                            status = request.Status,
                            priority = request.Priority,
                            employee = request.Employee
                        }, cancellationToken);

                        foreach (Tree bug in bugs)
                        {
                            if (program.Children == null) program.Children = new List<Tree>();
                            program.Children.Add(bug);
                        }
                    }
                }
                return trees;
            }
        }
    }
}
