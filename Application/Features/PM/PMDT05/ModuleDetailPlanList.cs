using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Application.Features.PM.PMDT05
{
    public class ModuleDetailPlanList
    {
        public class Query : IRequest<Data>
        {

        }

        public class Data
        {
            public List<Column> Columns { get; set; }
            public IEnumerable<MDP> Mdps { get; set; }
        }

        public class Column
        {
            public string Property { get; set; }
            public string ColumnName { get; set; }
        }

        public class MDP : PmModuleDetailPlanProgramAssign
        {
            public string ModuleCode { get; set; }
            public string ModuleName { get; set; }
            public string ProgramName { get; set; }
            public string Scope { get; set; }
            public string phase { get; set; }
            public string Manday { get; set; }
            public int EmployeeAssignId { get; set; }
            public string EmployeeAssign { get; set; }
            public string EmployeeCode { get; set; }
            public int ModuleId { get; set; }
            public int ProgramId { get; set; }
            public float? P25 { get; set; }
            public float? P26 { get; set; }
            public float? P27 { get; set; }
            public float? P28 { get; set; }
            public float? P29 { get; set; }
            public float? P30 { get; set; }
            public float? P31 { get; set; }
            public float? P32 { get; set; }
            public float? P33 { get; set; }
            public float? P34 { get; set; }
            public float? P35 { get; set; }
            public float? P36 { get; set; }
            public float? P37 { get; set; }
            public float? P38 { get; set; }
            public float? P39 { get; set; }
            public float? P40 { get; set; }
            public float? P41 { get; set; }
            public float? P42 { get; set; }
            public float? P43 { get; set; }
            public float? P44 { get; set; }
            public float? P45 { get; set; }
            public float? P46 { get; set; }
            public float? P47 { get; set; }
            public float? P48 { get; set; }
        }

        public class Handler : IRequestHandler<Query, Data>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Data> Handle(Query request, CancellationToken cancellationToken)
            {
                Data data = new Data();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"
                    select distinct	pmdp.module_detail_plan_code module_code,
				                    concat(pmdp.module_detail_plan_code, ' : ', get_wording_lang('th', pmdp.module_detail_plan_name_th, module_detail_plan_name_en)) module_name,
				                    concat(pmdpp.program_code, ' : ', get_wording_lang('th', pmdpp.program_name_th, pmdpp.program_name_en)) program_name,
				                    get_wording_lang('th', dli.list_item_short_name_tha, dli.list_item_short_name_eng) scope,
				                    null phase,
				                    pmdpp.estimate_md man_day,
				                    null employee_code,
                                    pmdp.module_detail_plan_id module_id,
				                    pmdpp.module_detail_plan_program_id program_id
                    from			pm_module_detail_plan pmdp
                    inner join		pm_module_detail_plan_program pmdpp
                    on				pmdpp.module_detail_plan_id = pmdp.module_detail_plan_id
                    inner join		pm_module_detail_plan_program_assign pmdppa
                    on				pmdppa.module_detail_plan_program_id = pmdpp.module_detail_plan_program_id
                    left join		db_list_item dli
                    on				dli.list_item_code = pmdpp.scope_type");
                data.Mdps = await _context.QueryAsync<MDP>(sql.ToString(), null, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"
                    select distinct	pmdp.module_detail_plan_code module_code,
				                    concat(de.employee_code, ' : ', get_full_employee_name('th', de.employee_code)) employee_assign,
                                    pmdp.module_detail_plan_id module_id,
				                    pmdpp.module_detail_plan_program_id program_id,
                                    pmdppa.employee_code_assign employee_code
                    from			pm_module_detail_plan pmdp
                    inner join		pm_module_detail_plan_program pmdpp
                    on				pmdpp.module_detail_plan_id = pmdp.module_detail_plan_id
                    inner join		pm_module_detail_plan_program_assign pmdppa
                    on				pmdppa.module_detail_plan_program_id = pmdpp.module_detail_plan_program_id
                    inner join		db_employee de
                    on				de.employee_code = pmdppa.employee_code_assign");
                IEnumerable<MDP> employees = await _context.QueryAsync<MDP>(sql.ToString(), null, cancellationToken);

                data.Columns = new List<Column>();
                object value;
                string property = string.Empty;
                string propertyForGetValue = string.Empty;
                

                foreach (MDP employee in employees)
                {
                    List<PmModuleDetailPlanProgramAssign> assigns = await _context.Set<PmModuleDetailPlanProgramAssign>().Where(w => w.ModuleDetailPlanProgramId == employee.ProgramId && w.EmployeeCodeAssign == employee.EmployeeCode).ToListAsync(cancellationToken);
                    int loop = assigns.Count * 24, index = 1;

                    foreach (PmModuleDetailPlanProgramAssign assign in assigns)
                    {
                        int phase = 1;
                        int checkPhase = 1;

                        for (int i = 1; i <= 24; i++)
                        {
                            property = string.Format("P{0}", index.ToString("00"));
                            propertyForGetValue = string.Format("P{0}", i.ToString("00"));
                            value = assign.GetType().GetProperty(propertyForGetValue).GetValue(assign);

                            if (!(value is null))
                            {
                                if (!data.Columns.Any(a => a.Property == property))
                                {
                                    data.Columns.Add(new Column()
                                    {
                                        Property = property,
                                        ColumnName = string.Format("{0}/{1} ({2})", (i % 2) == 0 ? "31" : "15", phase, assign.Year.Substring(2, 2))
                                    });
                                }

                                employee.GetType().GetProperty(property).SetValue(employee, value);
                            }

                            if (checkPhase == 2)
                            {
                                phase++;
                                checkPhase = 1;
                            }
                            else
                            {
                                checkPhase++;
                            }

                            index++;
                        }
                    }
                }

                data.Mdps = data.Mdps.Concat(employees).OrderBy(o => o.ModuleId).ThenBy(o => o.ProgramId).ToList();
                data.Columns = data.Columns.OrderBy(o => o.Property).ToList();
                return data;
            }
        }
    }
}
