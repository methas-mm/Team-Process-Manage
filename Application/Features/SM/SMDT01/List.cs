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

namespace Application.Features.SM.SMDT01
{
    public class List
    {
        public class ListData
        {
            public IEnumerable<dynamic> TableProject { get; set; }
            public IEnumerable<dynamic> TableWork { get; set; }
            public IEnumerable<dynamic> Month { get; set; }
            public dynamic Md { get; set; }
            public IEnumerable<MDofYear> TableMD { get; set; }
            public dynamic Leav { get; set; }

        }
        public class MDofYear
        {
            public string Month { get; set; }
            public float? EstmateMdPc { get; set; }
            public float? EstmateMdTw { get; set; }
            public float? AcualNonOther { get; set; }
            public float? AcualOther { get; set; }
        }



        public class Query : IRequest<ListData>
        {

        }

        public class Handler : IRequestHandler<Query, ListData>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;

            }

            public async Task<ListData> Handle(Query request, CancellationToken cancellationToken)
            {



                var ListData = new ListData();
                StringBuilder sql = new StringBuilder();
                sql = new StringBuilder();
                sql.AppendLine(@" select * from (
                                        select 
                                        pp.customer_id as customerId , 
                                        get_wording_lang('th',pc.customer_name_th,pc.customer_name_en) as customername , 
                                        pmpa.estimate_md as estmdcustomer , 
                                        sum(ptw.actual_md) as actualmdcustomer, 
                                        null as projectId,
                                        null as projectname,
                                        null as estmdproject,
                                        null as actualmdproject,
                                        null as masterplanId,
                                        null as modulename,
                                        0 as estmdModule,
                                        0 as actualModule,
                                        null as programId,
										null as programname,
										0 as estmdprogram,
										0 as actualmdprogram
                                        from pm_customer pc  
                                        inner join pm_project pp on pp.customer_id  = pc.customer_id 
                                        left join pm_master_plan pmp  on pmp.project_id  = pp.project_id
                                        left join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id 
                                        left join pm_task_work ptw on ptw.project_id = pp.project_id  
                                        group by customerId, customername  , estmdcustomer
                                        union
                                        select 
                                        pc.customer_id as customerId,
                                        null as customername,
                                        null as estmdcustomer,
                                        null as actualmdcustomer,
                                        pp.project_id as projectId , 
                                        get_wording_lang('th',pp.project_name_th ,pp.project_name_en) as projectname , 
                                        sum(pmdp.estimate_md) as estmdproject , 
                                        sum(ptw.actual_md) as actualmdproject,
                                        null as masterplanId,
                                        null as modulename,
                                        0 as estmdModule,
                                        0 as actualModule,
                                        0 as programId,
										null as programname,
										0 as estmdprogram,
										0 as actualmdprogram
                                        from pm_project pp 
                                        left join pm_customer pc on pc.customer_id = pp.customer_id 
                                        left join pm_master_plan pmp  on pmp.project_id  = pp.project_id
                                        left join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id 
                                        left join pm_module_detail_plan pmdp  on pmdp.master_plan_id = pmp.master_plan_id 
                                        left join pm_task_work ptw on ptw.project_id = pp.project_id  
                                        group by pc.customer_id , pp.project_id  ,projectname 
      									union
                                        select
                                        pp.customer_id as customerId,
                                        null as customername,
                                        0 as estmdcustomer,
                                        0 as actualmdcustomer,
                                        0 as projectId,
                                        null as projectname , 
                                        0 as estmdproject,
                                        0 as actualmdproject,
										pmp.master_plan_id::text as masterplanId ,  
										get_wording_lang('th',pmdp.module_detail_plan_name_th ,pmdp.module_detail_plan_name_en) as modulename , 
										pmdp.estimate_md as estmdModule, 
										sum(ptw.actual_md) as actualModule,
										0 as programId,
										null as programname,
										0 as estmdprogram,
										0 as actualmdprogram
                                        from pm_project pp 
                                        left join pm_master_plan pmp  on pmp.project_id  = pp.project_id
                                        left join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id 
                                        left join pm_module_detail_plan pmdp  on pmdp.master_plan_id = pmp.master_plan_id 
                                        left join pm_task_work ptw on ptw.project_id = pp.project_id  
                                        group by customerId,customername,estmdcustomer,actualmdcustomer,projectId,projectname,estmdproject,actualmdproject,masterplanId,modulename,estmdModule,programId,programname,estmdprogram,actualmdprogram
                                        union
                                        select 
                                        pp.customer_id as customerId,
                                        null as customername,
                                        null as estmdcustomer,
                                        null as actualmdcustomer,
                                        pp.project_id as projectId,
                                        null as projectname,
                                        null as estmdproject,
                                        null as actualmdproject,
                                        null as masterplanId,
                                        null as modulename,
                                        0 as estmdModule,
                                        0 as actualModule,
                                        pmdpp.module_detail_plan_program_id as programId, 
                                        get_wording_lang('th',pmdpp.program_name_th ,pmdpp.program_name_en) as programname , 
                                        pmdpp.estimate_md as estmdprogram , 
                                        sum(ptw.actual_md) as actualmdprogram 
                                        from pm_project pp 
                                        left join pm_master_plan pmp  on pmp.project_id  = pp.project_id
                                        left join pm_master_plan_assign pmpa on pmpa.master_plan_id = pmp.master_plan_id 
                                        left join pm_module_detail_plan pmdp  on pmdp.master_plan_id = pmp.master_plan_id 
                                        left join pm_module_detail_plan_program pmdpp on pmdpp.module_detail_plan_id = pmdp.module_detail_plan_id 
                                        left join pm_task_work ptw on ptw.project_id = pp.project_id  
                                       where pmdpp.module_detail_plan_program_id  is not null
                                       group by pmdpp.module_detail_plan_program_id , pmdpp.program_name_th , pmdpp.estimate_md ,customerId,projectId ) as a 
                                      order by customername , projectname, masterplanid, programid ");
                ListData.TableProject = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine($@"select * from(
                                    select 
                                     get_wording_lang('th',pc.customer_name_th,pc.customer_name_en) as customerName ,
                                     get_wording_lang('th',pp.project_name_th ,pp.project_name_en) as projectname ,
                                     null as workcode,
                                     null as workcodeName,
                                     null as program,
                                     null as subject,
                                     null as estmd,
                                     null as actualmd,
                                     null as startdate,
                                     null as enddate,
                                     ptw.project_id as projectid,
                                     null as status,
                                     pmp.master_plan_id as masterplanid
                                     from pm_customer pc 
                                    inner join pm_project pp on pc.customer_id = pp.customer_id 
                                    inner join pm_master_plan pmp on pmp.project_id = pp.project_id 
                                    inner join pm_module_detail_plan pmdp on pmp.master_plan_id = pmdp.master_plan_id 
                                    inner join pm_task_work ptw on ptw.module_detail_plan_id = pmdp.module_detail_plan_id
                                    left join pm_module_detail_plan_program pmdpp on pmdpp.module_detail_plan_program_id = ptw.program_id 
                                    left join db_status ds on ds.status_value  = ptw.status
                                    inner join st_user su on su.employee_code = ptw.employee_code_assign
                                    where su.user_id = '{this._user.UserId}' 
                                    group by customername ,projectname,masterplanid,projectid
                                    union
                                    select
                                     get_wording_lang('th',pc.customer_name_th,pc.customer_name_en) as customername ,
                                     null as projectname ,
                                     ptw.workcode_id as workcode,
                                     get_wording_lang('th',pw.workcode_name_th,pw.workcode_name_en) as workcodeName,
                                     get_wording_lang('th',pmdpp.program_name_th,pmdpp.program_name_en) as program,
                                     ptw.task_name as subject,
                                     pmdpp.estimate_md::text as estmd,
                                     ptw.actual_md::text as actualmd,
                                     ptw.start_date as startdate , 
                                     ptw.end_date as enddate,
                                     null as projectid,
                                     get_wording_lang('th', ds.status_desc_th , ds.status_desc_en)  as status ,
                                     pmp.master_plan_id as masterplanid
                                    from pm_customer pc 
                                    inner join pm_project pp on pc.customer_id = pp.customer_id 
                                    inner join pm_master_plan pmp on pmp.project_id = pp.project_id 
                                    inner join pm_module_detail_plan pmdp on pmp.master_plan_id = pmdp.master_plan_id 
                                    inner join pm_task_work ptw on ptw.module_detail_plan_id = pmdp.module_detail_plan_id
                                    inner join pm_workcode pw  on pw.workcode_id = ptw.workcode_id
                                    left join pm_module_detail_plan_program pmdpp on pmdpp.module_detail_plan_program_id = ptw.program_id 
                                    left join db_status ds on ds.status_value  = ptw.status
                                    inner join st_user su on su.employee_code = ptw.employee_code_assign
                                    where su.user_id = '{this._user.UserId}' 
                                    group by customername ,projectname,masterplanid,program,estmd,actualmd,workcode,subject,startdate,enddate,ds.status_desc_th , ds.status_desc_en,ptw.employee_code_assign,workcodeName
                                    ) as foo
                                    order by foo.customername,foo.masterplanid,foo.projectname");
                ListData.TableWork = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, this._user.UserId }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine($@"select ptw.actual_md as leav from pm_task_work ptw 
                        inner join pm_workcode pw on ptw.workcode_id = pw.workcode_id
                        inner join db_employee de  on de.employee_code  = ptw.employee_code_assign
                        left join st_user su on su.employee_code = de.employee_code
                        where  su.user_id = '{this._user.UserId}' and  ptw.workcode_group_id = '3' and pw.workcode_code  like '%LV%' ");
                ListData.Leav = await _context.QueryFirstOrDefaultAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, this._user.UserId }, cancellationToken);
                if (ListData.Leav != null)
                {
                    ListData.Leav = ListData.Leav;
                }
                else
                {
                    ListData.Leav = 0;
                }

                sql = new StringBuilder();
                sql.AppendLine(@"select list_item_code as value ,get_wording_lang('th', list_item_name_tha ,list_item_name_eng) as text from db_list_item
                                   where list_item_group_code = 'Month' order by list_item_code ");
                ListData.Month = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine($@"select 
                                    ccd.employee_code,
                                    cc.year,
                                    ccd.m01,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 1) as estmd1,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 1) as mano1,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 1) as mao1,
                                    ccd.m02,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 2) as estmd2,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 2) as mano2,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 2) as mao2,
                                    ccd.m03,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 3) as estmd3,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 3) as mano3,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 3) as mao3,
                                    ccd.m04,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 4) as estmd4,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 4) as mano4,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 4) as mao4,
                                    ccd.m05,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 5) as estmd5,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 5) as mano5,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 5) as mao5,
                                    ccd.m06,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 6) as estmd6,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 6) as mano6,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 6) as mao6,
                                    ccd.m07,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 7) as estmd7,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 7) as mano7,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 7) as mao7,
                                    ccd.m08,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 8) as estmd8,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 8) as mano8,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 8) as mao8,
                                    ccd.m09,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 9) as estmd9,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 9) as mano9,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 9) as mao9,
                                    ccd.m10,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 10) as estmd10,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 10) as mano10,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 10) as mao10,
                                    ccd.m11,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 11) as estmd11,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 11) as mano11,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 11) as mao11,
                                    ccd.m12,
                                    sum(ptw.estimate_md)filter(where extract(month from ptw.end_date)  = 12) as estmd12,
                                    sum(ptw.actual_md)filter(where workcode_group_id <> 3 and extract(month from ptw.end_date)  = 12) as mano12,
                                    sum(ptw.actual_md)filter(where workcode_group_id = 3 and extract(month from ptw.end_date)  = 12) as mao12
                                    from cp_capacity cc 
                                    inner join cp_capacity_detail ccd ON cc.capacity_id  = ccd.capacity_id 
                                    inner join pm_task_work ptw on ptw.employee_code_assign = ccd.employee_code
                                    left join st_user su on su.employee_code = ccd.employee_code 
                                    where su.user_id = '{this._user.UserId}' and cc.year = '2564'
                                    group by ccd.employee_code,cc.year,ccd.m01,ccd.m02,ccd.m03,ccd.m04,ccd.m05,
                                    ccd.m06,ccd.m07,ccd.m08,ccd.m09,ccd.m10,ccd.m11,ccd.m12
                                    ");
                ListData.Md = await _context.QueryFirstOrDefaultAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, this._user.UserId }, cancellationToken);

                List<MDofYear> MOY = new List<MDofYear>();
                foreach (var item in ListData.Month)
                {
                    MDofYear mDofYear = new MDofYear();
                    mDofYear.Month = item.text;
                    if (ListData.Md != null)
                    {
                        if (item.value == "01")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m01 == null ? 0 : ListData.Md.m01;
                            mDofYear.EstmateMdTw = ListData.Md.estmd1 == null ? 0 : ListData.Md.estmd1;
                            mDofYear.AcualNonOther = ListData.Md.mano1 == null ? 0 : ListData.Md.mano1;
                            mDofYear.AcualOther = ListData.Md.mao1 == null ? 0 : ListData.Md.mao1;
                        }
                        else if (item.value == "02")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m02 == null ? 0 : ListData.Md.m02;
                            mDofYear.EstmateMdTw = ListData.Md.estmd2 == null ? 0 : ListData.Md.estmd2;
                            mDofYear.AcualNonOther = ListData.Md.mano2 == null ? 0 : ListData.Md.mano2;
                            mDofYear.AcualOther = ListData.Md.mao2 == null ? 0 : ListData.Md.mao2;
                        }

                        else if (item.value == "03")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m03 == null ? 0 : ListData.Md.m03;
                            mDofYear.EstmateMdTw = ListData.Md.estmd3 == null ? 0 : ListData.Md.estmd3;
                            mDofYear.AcualNonOther = ListData.Md.mano3 == null ? 0 : ListData.Md.mano3;
                            mDofYear.AcualOther = ListData.Md.mao3 == null ? 0 : ListData.Md.mao3;
                        }
                        else if (item.value == "04")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m04 == null ? 0 : ListData.Md.m04;
                            mDofYear.EstmateMdTw = ListData.Md.estmd4 == null ? 0 : ListData.Md.estmd4;
                            mDofYear.AcualNonOther = ListData.Md.mano4 == null ? 0 : ListData.Md.mano4;
                            mDofYear.AcualOther = ListData.Md.mao4 == null ? 0 : ListData.Md.mao4;
                        }
                        else if (item.value == "05")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m05 == null ? 0 : ListData.Md.m05;
                            mDofYear.EstmateMdTw = ListData.Md.estmd5 == null ? 0 : ListData.Md.estmd5;
                            mDofYear.AcualNonOther = ListData.Md.mano5 == null ? 0 : ListData.Md.mano5;
                            mDofYear.AcualOther = ListData.Md.mao5 == null ? 0 : ListData.Md.mao5;
                        }
                        else if (item.value == "06")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m06 == null ? 0 : ListData.Md.m06;
                            mDofYear.EstmateMdTw = ListData.Md.estmd6 == null ? 0 : ListData.Md.estmd6;
                            mDofYear.AcualNonOther = ListData.Md.mano6 == null ? 0 : ListData.Md.mano6;
                            mDofYear.AcualOther = ListData.Md.mao6 == null ? 0 : ListData.Md.mao6;
                        }
                        else if (item.value == "07")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m07 == null ? 0 : ListData.Md.m07;
                            mDofYear.EstmateMdTw = ListData.Md.estmd7 == null ? 0 : ListData.Md.estmd7;
                            mDofYear.AcualNonOther = ListData.Md.mano7 == null ? 0 : ListData.Md.mano7;
                            mDofYear.AcualOther = ListData.Md.mao7 == null ? 0 : ListData.Md.mao7;
                        }
                        else if (item.value == "08")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m08 == null ? 0 : ListData.Md.m08;
                            mDofYear.EstmateMdTw = ListData.Md.estmd8 == null ? 0 : ListData.Md.estmd8;
                            mDofYear.AcualNonOther = ListData.Md.mano8 == null ? 0 : ListData.Md.mano8;
                            mDofYear.AcualOther = ListData.Md.mao8 == null ? 0 : ListData.Md.mao8;
                        }
                        else if (item.value == "09")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m09 == null ? 0 : ListData.Md.m09;
                            mDofYear.EstmateMdTw = ListData.Md.estmd9 == null ? 0 : ListData.Md.estmd9;
                            mDofYear.AcualNonOther = ListData.Md.mano9 == null ? 0 : ListData.Md.mano9;
                            mDofYear.AcualOther = ListData.Md.mao9 == null ? 0 : ListData.Md.mao9;
                        }
                        else if (item.value == "10")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m10 == null ? 0 : ListData.Md.m10;
                            mDofYear.EstmateMdTw = ListData.Md.estmd10 == null ? 0 : ListData.Md.estmd10;
                            mDofYear.AcualNonOther = ListData.Md.mano10 == null ? 0 : ListData.Md.mano10;
                            mDofYear.AcualOther = ListData.Md.mao10 == null ? 0 : ListData.Md.mao10;
                        }
                        else if (item.value == "11")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m11 == null ? 0 : ListData.Md.m11;
                            mDofYear.EstmateMdTw = ListData.Md.estmd11 == null ? 0 : ListData.Md.estmd11;
                            mDofYear.AcualNonOther = ListData.Md.mano11 == null ? 0 : ListData.Md.mano11;
                            mDofYear.AcualOther = ListData.Md.mao11 == null ? 0 : ListData.Md.mao11;
                        }
                        else if (item.value == "12")
                        {
                            mDofYear.EstmateMdPc = ListData.Md.m12 == null ? 0 : ListData.Md.m12;
                            mDofYear.EstmateMdTw = ListData.Md.estmd12 == null ? 0 : ListData.Md.estmd12;
                            mDofYear.AcualNonOther = ListData.Md.mano12 == null ? 0 : ListData.Md.mano12;
                            mDofYear.AcualOther = ListData.Md.mao12 == null ? 0 : ListData.Md.mao12;
                        }
                    }
                    else
                    {
                        mDofYear.EstmateMdPc = 0;
                        mDofYear.EstmateMdTw = 0;
                        mDofYear.AcualNonOther = 0;
                        mDofYear.AcualOther = 0;
                    }

                    MOY.Add(mDofYear);
                }
                ListData.TableMD = MOY;


                return ListData;
            }
        }
    }
}
