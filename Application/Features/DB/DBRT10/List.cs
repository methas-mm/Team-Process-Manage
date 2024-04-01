using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using System.Threading;
using Domain.Entities.DB;

namespace Application.Features.DB.DBRT10
{
    public class List
    {

        public class Query : RequestPageQuery, IRequest<EmployeeDTO>
        {
            public string Keyword { get; set; }
        }
        public class DbEmployee2 : DbEmployee
        {
            public string TeamName { get; set; }
            public string EmployeeName { get; set; }

        }

        public class EmployeeDTO
        {
            public IEnumerable<DbEmployee2> EmployeeList { get; set; }
        }

        public class Handler : IRequestHandler<Query, EmployeeDTO>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;

            }

            public async Task<EmployeeDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select 
                de.employee_code as ""EmployeeCode"", 
                case @lang when 'th' then concat(pre.list_item_name_tha, de.first_name_th, ' ', de.last_name_th)
                else concat(pre.list_item_name_eng, de.first_name_en, ' ', de.last_name_en) end as ""EmployeeName"",
                get_wording_lang(@lang, pos.list_item_name_tha, pos.list_item_name_eng) as ""PositionId"", 
                get_wording_lang(@lang, gen.list_item_name_tha, gen.list_item_name_eng) as ""Gender"", 
                get_wording_lang(@lang, dt.team_name_th, dt.team_name_eng) as ""TeamName"", 
                de.xmin AS ""rowVersion""
                from db_employee de
                left
                join db_list_item gen on gen.list_item_code = de.gender and gen.list_item_group_code = 'Gender'
                left join db_list_item pos on pos.list_item_code = de.position_id and pos.list_item_group_code = 'Position'
                left join db_list_item pre on pre.list_item_code = de.prefix_id and pre.list_item_group_code = 'PrefixName'
                left join db_team dt on dt.team_id = de.team_id
                where 1=1");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine($@"and  concat(de.employee_code
                ,pre.list_item_name_tha
                ,pre.list_item_name_eng
                ,de.first_name_th
                ,de.first_name_en
                ,de.last_name_th
                ,de.last_name_en
                ,pos.list_item_name_tha
                ,pos.list_item_name_eng
                ,gen.list_item_name_tha
                ,gen.list_item_name_eng
                ,dt.team_name_th
                ,dt.team_name_eng) ilike concat('%{request.Keyword}%')");
                }
                sql.AppendLine("order by de.employee_code");

                EmployeeDTO Model = new EmployeeDTO();
                Model.EmployeeList = await _context.QueryAsync<DbEmployee2>(sql.ToString(), new { lang = this._user.Language, keyword = request.Keyword }, cancellationToken);
                return Model;
            }

        }

    }
}