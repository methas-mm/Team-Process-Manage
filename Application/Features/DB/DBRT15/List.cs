using MediatR;
using Application.Interfaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using Domain.Entities.DB;
using System.Collections.Generic;

namespace Application.Features.DB.DBRT15
{
    public class List
    {
       
        public class Query : RequestPageQuery, IRequest<HolidayDTO>
        {
            public string Year { get; set; }
            public int? Month { get; set; }
        }
        public class HolidayDTO : DbHoliday 
        {
            public IEnumerable<DbHoliday> HolidayList { get; set; }
        }

        public class Handler : IRequestHandler<Query, HolidayDTO>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<HolidayDTO> Handle(Query request, CancellationToken cancellationToken)
             {
                StringBuilder sql = new StringBuilder();
 
             
                sql.AppendLine("select ");
                sql.AppendLine("holiday_id as \"holidayId\", ");
                sql.AppendLine("holiday_date as \"holidayDate\", ");
                sql.AppendLine("holiday_end_date as \"holidayEndDate\", ");
                sql.AppendLine("holiday_name as \"holidayName\", ");
                sql.AppendLine("holiday_desc as \"holidayDesc\", ");
                sql.AppendLine("active as \"active\", ");
                sql.AppendLine("xmin as \"rowVersion\" ");
                sql.AppendLine("from db_holiday ");
                sql.AppendLine("where 1=1 ");
                if (request.Month!=null)
                {
                    sql.AppendLine("and  EXTRACT(MONTH FROM holiday_date  )::int = @Month ");
                }
                if (!string.IsNullOrEmpty(request.Year))
                {
                    sql.AppendLine("and  EXTRACT(YEAR FROM holiday_date  )::text = @Year ");
                }
                sql.AppendLine("ORDER BY holiday_date asc");
                string year = string.Empty;
                if (!string.IsNullOrEmpty(request.Year)) 
                {
                 year = int.Parse(request.Year) > 2500 ? (int.Parse(request.Year) - 543).ToString() : request.Year;

                }
                HolidayDTO data = new HolidayDTO();
                 data.HolidayList = await _context.QueryAsync<HolidayDTO>(sql.ToString(), new { Language = this._user.Language, Year = year, Month = request.Month },cancellationToken);
                return data;
            }
        }
    }
}


