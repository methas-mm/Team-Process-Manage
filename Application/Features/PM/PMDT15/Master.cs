using Application.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class Master
    {
        public class MasterList
        {
            public IEnumerable<dynamic> Customer { get; set; }
            public IEnumerable<dynamic> ProjectName { get; set; }
            public IEnumerable<dynamic> WorkGroupCode { get; set; }
            public IEnumerable<dynamic> WorkCode { get; set; }
            public IEnumerable<dynamic> ModuleDetailPlan { get; set; }
            public IEnumerable<dynamic> ProgramName { get; set; }
            public IEnumerable<dynamic> Status { get; set; }
            public IEnumerable<dynamic> ResponsibleName { get; set; }
            public string EmployeeCode { get; set; }
        }
        public class Query : IRequest<MasterList>
        {

        }
        public class ProfileDTO
        {
            public int TeamId { get; set; }
            public string LeaderCode { get; set; }
            public string EmployeeCode { get; set; }
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
                StringBuilder sql;
                sql = new StringBuilder();
                sql.AppendLine(@"select de.team_id as ""TeamId"" ,dt.employee_code as ""LeaderCode"",de.employee_code as ""EmployeeCode""
                                from db_employee de 
                                inner join st_user su on su.employee_code = de.employee_code 
                                 left join db_team dt ON de.team_id = dt.team_id 
                                where su.user_id = @id");
                ProfileDTO EmpData = await _context.QuerySingleOrDefaultAsync<ProfileDTO>(sql.ToString(), new { id = this._user.UserId }, cancellationToken);
                master.EmployeeCode = EmpData.EmployeeCode;

                sql = new StringBuilder();
                sql.AppendLine(@"select pc.customer_id as value,
                                 concat(pc.customer_code ,' : ',get_wording_lang(@lang,pc.customer_name_th ,pc.customer_name_en)) as text
                                 from pm_customer pc 
                                 where active = true 
                                 and company_code = @company");
                master.Customer = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                 pp.customer_id as ""CustomerId"",
                                 pp.project_id as value,
                                 concat(pp.project_code,' : ',get_wording_lang(@lang,pp.project_name_th ,pp.project_name_en)) as text
                                 from pm_project pp 
                                 inner join pm_customer pc on pc.customer_id = pp.customer_id 
                                 where pc.active = true 
                                 and pc.company_code  = @company");
                master.ProjectName = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select pwg.workcode_group_id as value,
                                 concat(pwg.workcode_group_code ,' : ',get_wording_lang(@lang,pwg.workgroup_name_th ,pwg.workgroup_name_en)) as text
                                 from pm_workcode_group pwg 
                                 order by pwg.workcode_group_code ");
                master.WorkGroupCode = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select pw.workcode_group_id as ""WorkCodeGroupId"",
                                        pw.workcode_id as value,
                                        concat(pw.workcode_code,' : ',get_wording_lang(@lang,pw.workcode_name_th ,pw.workcode_name_en)) as text
                                        from pm_workcode_group pwg 
                                        inner join pm_workcode pw on pw.workcode_group_id = pwg.workcode_group_id 
                                        order by pw.workcode_code");
                master.WorkCode = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select 
								 pmdp.module_detail_plan_id as value,
                                 concat(pmdp.module_detail_plan_code,' : ',get_wording_lang(@lang,pmdp.module_detail_plan_name_th,pmdp.module_detail_plan_name_en)) as text ,
                                 pw.workcode_id as ""WorkCode"",
                                 pwg.workcode_group_id as ""WorkCodeGroup"",
                                 pp.project_id as ""ProjectId"",
                                 pc.customer_id as ""CustomerId""
                                 from pm_module_detail_plan pmdp  
                                 inner join pm_master_plan pmp on pmp.master_plan_id  = pmdp.master_plan_id 
                                 inner join pm_project pp  on pp.project_id  = pmp.project_id  
                                 inner join pm_customer pc  on pc.customer_id  = pp.customer_id 
                                 inner join pm_workcode_group pwg  on pwg.workcode_group_id  = pmp.workcode_group_id
                                 inner join pm_workcode pw  on pw.workcode_id  = pmp.workcode_id 
                                 where pc.active = true
                                 and pc.company_code = @company ");
                master.ModuleDetailPlan = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select 
                                 pmdpp.module_detail_plan_program_id as value, 
                                 concat('[',pmdp.module_detail_plan_code,'][',pmdpp.program_code,'] : ',get_wording_lang(@lang,pmdpp.program_name_th,pmdpp.program_name_en)) as text,
                                 pmdp.module_detail_plan_id as ""ModuleDetailPlanId"",
                                 pw.workcode_id as ""WorkCode"",
                                 pwg.workcode_group_id as ""WorkCodeGroup"",
                                 pp.project_id as ""ProjectId"",
                                 pc.customer_id as ""CustomerId"",
                                 pmdpp.estimate_md as ""EstimateMD""
                                 from pm_module_detail_plan_program pmdpp  
                                 inner join pm_module_detail_plan pmdp ON  pmdp.module_detail_plan_id  = pmdpp.module_detail_plan_id 
                                 inner join pm_master_plan pmp on pmp.master_plan_id  = pmdp.master_plan_id 
                                 inner join pm_project pp  on pp.project_id  = pmp.project_id  
                                 inner join pm_customer pc  on pc.customer_id  = pp.customer_id 
                                 inner join pm_workcode_group pwg  on pwg.workcode_group_id  = pmp.workcode_group_id
                                 inner join pm_workcode pw  on pw.workcode_id  = pmp.workcode_id 
                                 where pc.active = true
                                 and pc.company_code = @company
                                 ");
                sql.AppendLine("order by pmdp.module_detail_plan_code,pmdpp.program_code  ");
                master.ProgramName = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                master.Status = await _context.Set<DbStatus>().OrderBy(i => i.Sequence).Where(i => i.TableName == "pm_task_sub" && i.ColumnName == "status" && i.Active == true).Select(s => new
                {
                    value = s.StatusValue,
                    text = this._user.Language == "th" ? s.StatusDescTh : s.StatusDescEn,
                }).ToListAsync(cancellationToken);

                // Role Leader
                if (EmpData.LeaderCode == EmpData.EmployeeCode)
                {
                    master.ResponsibleName = await _context.Set<DbEmployee>().Where(i => i.TeamId == EmpData.TeamId).Select(s => new
                    {
                        value = s.EmployeeCode,
                        text = this._user.Language == "th" ? s.FirstNameTh + " " + s.LastNameTh : s.FirstNameEn + " " + s.LastNameEn
                    }).ToListAsync(cancellationToken);
                }
                // Role Crewmate
                else
                {
                    master.ResponsibleName = await _context.Set<DbEmployee>().Where(i => i.EmployeeCode == EmpData.EmployeeCode).Select(s => new
                    {
                        value = s.EmployeeCode,
                        text = this._user.Language == "th" ? s.FirstNameTh + " " + s.LastNameTh : s.FirstNameEn + " " + s.LastNameEn
                    }).ToListAsync(cancellationToken);
                }

                return master;
            }
        }
    }
}
