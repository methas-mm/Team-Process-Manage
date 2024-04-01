using Application.Interfaces;
using Domain.Entities.CP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CP.CPDT05
{

    public class Detail
    {
        public class Query : IRequest<IEnumerable<CapacityDetailDTO>>
        {
            public int Id { get; set; }
        }
        public class CapacityDetailDTO : CpCapacityDetail
        {
            public string EmployeeName { get; set; }
            public float? Jan { get; set; }
            public float? Feb { get; set; }
            public float? Mar { get; set; }
            public float? Apr { get; set; }
            public float? May { get; set; }
            public float? Jun { get; set; }
            public float? Jul { get; set; }
            public float? Aug { get; set; }
            public float? Sep { get; set; }
            public float? Oct { get; set; }
            public float? Nov { get; set; }
            public float? Dec { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<CapacityDetailDTO>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<CapacityDetailDTO>> Handle(Query request, CancellationToken cancellationToken)
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"   select 
                                     de.employee_code as ""EmployeeCode"",
                                     ccd.capacity_detail_id as ""CapacityDetailId"",
                                     get_full_employee_name(@lang,de.employee_code) as EmployeeName,
                                     ccd.m01,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '1' and extract(month from ptw.end_date)::text = '1' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Jan,
                                     ccd.m02,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '2' and extract(month from ptw.end_date)::text = '2' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Feb,
                                     ccd.m03,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '3' and extract(month from ptw.end_date)::text = '3' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Mar,
                                     ccd.m04,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '4' and extract(month from ptw.end_date)::text = '4' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Apr,
                                     ccd.m05,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '5' and extract(month from ptw.end_date)::text = '5' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as May,
                                     ccd.m06,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '6' and extract(month from ptw.end_date)::text = '6' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Jun,
                                     ccd.m07,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '7' and extract(month from ptw.end_date)::text = '7' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Jul,
                                     ccd.m08,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '8' and extract(month from ptw.end_date)::text = '8' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Aug,
                                     ccd.m09,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '9' and extract(month from ptw.end_date)::text = '9' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Sep,
                                     ccd.m10,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '10' and extract(month from ptw.end_date)::text = '10' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Oct,
                                     ccd.m11,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '11' and extract(month from ptw.end_date)::text = '11' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Nov,
                                     ccd.m12,
                                     (select sum( ptw.estimate_md) from pm_task_work ptw where ptw.employee_code_assign = de.employee_code and extract(month from ptw.start_date)::text = '12' and extract(month from ptw.end_date)::text = '12' and extract(year from ptw.start_date)::text = (cc.year::int - 543)::text and extract(year from ptw.end_date)::text = (cc.year::int - 543)::text and ptw.status = 'C') as Dec,
                                     cc.capacity_id  as  capacityId,ccd.xmin as ""rowVersion""
                                    from cp_capacity cc 
                                    inner join cp_capacity_detail ccd on cc.capacity_id = ccd.capacity_id 
                                    inner join db_employee de on ccd.employee_code = de.employee_code
                                    where cc.capacity_id = @CapacityDetailId
                                    group by EmployeeName,ccd.m01 ,ccd.m02 ,ccd.m03 ,ccd.m04 ,ccd.m05 ,ccd.m06 ,ccd.m07 ,ccd.m08 ,ccd.m09 ,ccd.m10 ,ccd.m11 ,ccd.m12,  cc.capacity_Id, ""EmployeeCode"",""CapacityDetailId"" 
                                    ORDER by EmployeeName ASC");

                var data = await _context.QueryAsync<CapacityDetailDTO>(sql.ToString(), new { CapacityDetailId = request.Id, lang = _user.Language });
                return data;
            }


        }
    }

}
