using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class MasterPlanAssignDetail
    {
        public class Query : IRequest<Data>
        {
            public int masterPlanId { get; set; }
        }
        public class Data
        {
            public string projectName { get; set; }
            public string customerName { get; set; }
            public Header Header { get; set; }
        }
        public class Header
        {
            public int WorkGroup { get; set; }
            public int WorkCode { get; set; }
            public string Year { get; set; }
            public IEnumerable<Detail> Details { get; set; }
        }
        public class Detail
        {
            public string Position { get; set; }
            public string Employee { get; set; }
            public Sub Sub { get; set; }
        }
        public class Sub
        {
            public float? p01 { get; set; }
            public float? p02 { get; set; }
            public float? p03 { get; set; }
            public float? p04 { get; set; }
            public float? p05 { get; set; }
            public float? p06 { get; set; }
            public float? p07 { get; set; }
            public float? p08 { get; set; }
            public float? p09 { get; set; }
            public float? p10 { get; set; }
            public float? p11 { get; set; }
            public float? p12 { get; set; }
            public float? p13 { get; set; }
            public float? p14 { get; set; }
            public float? p15 { get; set; }
            public float? p16 { get; set; }
            public float? p17 { get; set; }
            public float? p18 { get; set; }
            public float? p19 { get; set; }
            public float? p20 { get; set; }
            public float? p21 { get; set; }
            public float? p22 { get; set; }
            public float? p23 { get; set; }
            public float? p24 { get; set; }
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
                Data Data = new Data();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select  
	                                get_wording_lang(@lang,pp.project_name_th,pp.project_name_en) as ""projectName"",
	                                get_wording_lang(@lang,pc.customer_name_th,pc.customer_name_en) as ""customerName""
                                from pm_master_plan pmp 
                                inner join pm_project pp on pmp.project_id = pp.project_id 
                                inner join pm_customer pc on pc.customer_id = pp.customer_id 
                                where pmp.master_plan_id = @masterPlanId");
                Data = await _context.QueryFirstAsync<Data>(sql.ToString(), new { lang = _user.Language, masterPlanId = request.masterPlanId }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine(@"select * from pm_master_plan_assign where master_plan_id = @masterPlanId");
                //pmpa.PmMasterPlanAssigns = await _context.QueryAsync<PmMasterPlanAssign>(sql.ToString(), new { masterPlanId = request.masterPlanId }, cancellationToken);
                return Data;
            }
        }
    }
}
