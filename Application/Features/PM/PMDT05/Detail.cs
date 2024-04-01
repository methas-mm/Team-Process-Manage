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

namespace Application.Features.PM.PMDT05
{
    public class Detail
    {
        public class Query : IRequest<Data>
        {
            public int projectId { get; set; }
        }
        public class Data
        {
            public dynamic projects { get; set; }
            public List<Column> Columns { get; set; }
            public IEnumerable<MPA> Mpas { get; set; }
        }

        public class Column
        {
            public string Property { get; set; }
            public string ColumnName { get; set; }
        }

        public class MPA : PmMasterPlanAssign
        {
            public int MasterPlanId { get; set; }
            public string WorkGroup { get; set; }
            public string Workcode_code { get; set; }
            public string WorkCode { get; set; }
            public string Description { get; set; }
            public string Position { get; set; }
            public string Employee { get; set; }
            public string EmployeeCode { get; set; }
            public float? EstimateMd { get; set; }
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
                                select 
									pmp.master_plan_id as ""masterPlanId"",
									get_wording_lang(@lang,pwg.workgroup_name_th,pwg.workgroup_name_en) as ""workGroup"",
									pw.workcode_code,
									pw.workcode_code as ""workCode"",
									get_wording_lang(@lang,pw.workcode_name_th,pw.workcode_name_en) as description,
									sum(pmpa.p01) + sum(pmpa.p02) + sum(pmpa.p03) + sum(pmpa.p04) + sum(pmpa.p05) + sum(pmpa.p06) + sum(pmpa.p07) + sum(pmpa.p08) + sum(pmpa.p09) + sum(pmpa.p10) + sum(pmpa.p11) + sum(pmpa.p12) + sum(pmpa.p13) + sum(pmpa.p14) + sum(pmpa.p15) + sum(pmpa.p16) + sum(pmpa.p17) + sum(pmpa.p18) + sum(pmpa.p19) + sum(pmpa.p20) + sum(pmpa.p21) + sum(pmpa.p22) + sum(pmpa.p23) + sum(pmpa.p24) as ""estimateMd"",
									null as position,
									null as employee
								from pm_project pp 
								inner join pm_master_plan pmp on pp.project_id = pmp.project_id 
								inner join pm_workcode pw on pw.workcode_id = pmp.workcode_id 
								inner join pm_workcode_group pwg on pwg.workcode_group_id = pmp.workcode_group_id 
								inner join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id
								where pp.project_id = @projectId
								group by ""workGroup"",""workCode"",""workCode"",description ,""masterPlanId""");
                data.Mpas = await _context.QueryAsync<MPA>(sql.ToString(), new { lang = _user.Language, projectId = request.projectId }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"
                                select 
									pmp.master_plan_id as ""masterPlanId"",
									get_wording_lang(@lang,pwg.workgroup_name_th,pwg.workgroup_name_en) as ""workGroup"",
									pw.workcode_code,
									null as ""workCode"",
									null description,
									sum(pmpa.p01) + sum(pmpa.p02) + sum(pmpa.p03) + sum(pmpa.p04) + sum(pmpa.p05) + sum(pmpa.p06) + sum(pmpa.p07) + sum(pmpa.p08) + sum(pmpa.p09) + sum(pmpa.p10) + sum(pmpa.p11) + sum(pmpa.p12) + sum(pmpa.p13) + sum(pmpa.p14) + sum(pmpa.p15) + sum(pmpa.p16) + sum(pmpa.p17) + sum(pmpa.p18) + sum(pmpa.p19) + sum(pmpa.p20) + sum(pmpa.p21) + sum(pmpa.p22) + sum(pmpa.p23) + sum(pmpa.p24) as ""estimateMd"",
									get_wording_lang(@lang,dli.list_item_name_tha,dli.list_item_name_eng) as position,
									get_full_employee_name(@lang,de.employee_code) as employee,
									de.employee_code as ""employeeCode""
								from pm_project pp 
								inner join pm_master_plan pmp on pp.project_id = pmp.project_id 
								inner join pm_workcode pw on pw.workcode_id = pmp.workcode_id 
								inner join pm_workcode_group pwg on pwg.workcode_group_id = pmp.workcode_group_id 
								inner join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id 
								inner join db_employee de on de.employee_code = pmpa.employee_code_assign 
								inner join db_list_item dli on dli.list_item_group_code = 'Position' and dli.list_item_code = de.position_id 
								where pp.project_id = @projectId
								group by ""workGroup"",""workCode"",description,employee,pw.workcode_code,position,""masterPlanId"", ""employeeCode""");
                IEnumerable<MPA> employees = await _context.QueryAsync<MPA>(sql.ToString(), new { lang = _user.Language, projectId = request.projectId }, cancellationToken);

                data.Columns = new List<Column>();
                object value;
                string property = string.Empty;
                string propertyForGetValue = string.Empty;


                foreach (MPA employee in employees)
                {
                    List<PmMasterPlanAssign> assigns = await _context.Set<PmMasterPlanAssign>().Where(w => w.MasterPlanId == employee.MasterPlanId && w.EmployeeCodeAssign == employee.EmployeeCode).ToListAsync(cancellationToken);
                    int loop = assigns.Count * 24, index = 1;

                    foreach (PmMasterPlanAssign assign in assigns)
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

                data.Mpas = data.Mpas.Concat(employees).OrderBy(o => o.Workcode_code).ThenBy(o => o.MasterPlanAssignId).ToList();
                data.Columns = data.Columns.OrderBy(o => o.Property).ToList();

                sql = new StringBuilder();
                sql.AppendLine(@"select 
									pp.project_id as ""projectId"",
									get_wording_lang(@lang,pp.project_name_th,pp.project_name_en) as ""projectName"",
									get_wording_lang(@lang,pc.customer_name_th,pc.customer_name_en) as ""customerName""
								from pm_project pp 
								inner join pm_customer pc on pp.customer_id = pc.customer_id 
								where pp.project_id = @projectId");

                data.projects = await _context.QueryFirstOrDefaultAsync<dynamic>(sql.ToString(), new { lang = _user.Language, projectId = request.projectId }, cancellationToken);

                return data;
            }
        }
    }
}
