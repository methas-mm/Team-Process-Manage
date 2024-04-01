using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;

namespace Application.Features.DB.DBRT05
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Object>>
        {
            public string Keyword { get; set; }
        }
       
        public class Handler : IRequestHandler<Query, IEnumerable<Object>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<Object>> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select dtse.team_sub_employee_id as teamSubEmployeeId,dtse.team_id as teamId,dtse.employee_code as employeeCode,
                                 de.company_code as companyCode,de.employee_code as employeeCode,de.first_name_th as firstNameTh,de.first_name_en as firstNameEn, dt.team_id as teamId,dt.company_code as companyCode,dt.team_name_th as teamNameTh,dt.team_name_eng as teamNameEng,dt.employee_code as employeeCode,dt.team_code as teamCode
                                 from db_team_sub_employee dtse
                                  inner join db_team dt on dt.team_id = dtse.team_id
                                  inner join db_employee de on de.employee_code = dtse.employee_code");


                //if (!string.IsNullOrEmpty(request.Keyword))
                //{
                //    sql.AppendLine("WHERE       CONCAT(dt.company_code,");
                //    sql.AppendLine("                   dt.team_id,");
                //    sql.AppendLine("                   dt.team_code,");
                //    sql.AppendLine("                   dt.employee_code,");
                //    sql.AppendLine("                   dt.team_name_eng,");
                //    sql.AppendLine("                   dt.team_name_th)");
                //    sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                //}

                //sql.AppendLine(" order by dt.team_id");

                return  await _context.QueryAsync<dynamic>(sql.ToString(), new { Keyword = request.Keyword }, cancellationToken);
                 


            }


        }
    }
}
