using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT20
{
    public class Master
    {
        public class MasterList
        {
            public IEnumerable<dynamic> Customer { get; set; }
            public IEnumerable<dynamic> Project { get; set; }
            public IEnumerable<dynamic> Module { get; set; }
            public IEnumerable<dynamic> Program { get; set; }
            public IEnumerable<dynamic> Status { get; set; }
            public IEnumerable<dynamic> Priority { get; set; }
            public IEnumerable<dynamic> Employee { get; set; }
        }

        public class Query : IRequest<MasterList>
        {
            public string Field { get; set; }
            public int? Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, MasterList>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MasterList> Handle(Query request, CancellationToken cancellationToken)
            {
                MasterList master = new MasterList();
                switch (request.Field)
                {
                    case null:
                        master.Customer = await _context.QueryAsync<dynamic>(@"
                            select  customer_id as value,
                                    concat(customer_code, ' : ', get_wording_lang(@lang, customer_name_th, customer_name_en)) as text
                            from    pm_customer",
                            new { lang = _user.Language }, cancellationToken);
                        master.Status = await _context.QueryAsync<dynamic>(@"
                            select  status_value as value,
                                    get_wording_lang(@lang, status_desc_th, status_desc_en) as text
                            from    db_status
                            where   table_name = 'pm_task_sub'
                                    and column_name = 'status'",
                            new { lang = _user.Language }, cancellationToken);
                        master.Priority = await _context.QueryAsync<dynamic>(@"
                            select  status_value as value,
                                    get_wording_lang(@lang, status_desc_th, status_desc_en) as text
                            from    db_status
                            where   table_name = 'pm_task_sub'
                                    and column_name = 'priority'",
                            new { lang = _user.Language }, cancellationToken);
                        master.Employee = await _context.QueryAsync<dynamic>(@"
                            select  employee_code as value,
                                    concat(employee_code, ' : ', get_wording_lang(@lang, first_name_th, first_name_en), ' ', get_wording_lang(@lang, last_name_th, last_name_en)) as text
                            from    db_employee",
                            new { lang = _user.Language }, cancellationToken);
                        break;
                    case "project":
                        master.Project = await _context.QueryAsync<dynamic>(@"
                            select  project_id as value,
                                    concat(project_code, ' : ', get_wording_lang(@lang, project_name_th, project_name_en)) as text
                            from    pm_project
                            where   customer_id = @id",
                            new { lang = _user.Language, id = request.Id }, cancellationToken);
                        break;
                    case "module":
                        master.Module = await _context.QueryAsync<dynamic>(@"
                            select		concat(pmdp.module_detail_plan_code, ' : ', get_wording_lang('th', pmdp.module_detail_plan_name_th, pmdp.module_detail_plan_name_en)) as text,
			                            pmdp.module_detail_plan_id as value
                            from		pm_master_plan pmp
                            inner join	pm_module_detail_plan pmdp 
                            on			pmdp.master_plan_id = pmp.master_plan_id
                            where		pmp.project_id = @id",
                            new { lang = _user.Language, id = request.Id }, cancellationToken);
                        break;
                    case "program":
                        master.Program = await _context.QueryAsync<dynamic>(@"
                            select	module_detail_plan_program_id as value,
		                            concat(program_code, ' : ', get_wording_lang('th', program_name_th, program_name_en)) as text
                            from    pm_module_detail_plan_program
                            where   module_detail_plan_id = @id",
                            new { lang = _user.Language, id = request.Id }, cancellationToken);
                        break;
                }
                
                return master;
            }
        }
    }
}
