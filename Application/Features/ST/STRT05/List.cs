using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT05
{
    public class List
    {
        public class UserList : EntityBase
        {
            public long id { get; set; }
            public string userName { get; set; }
            public string employeeName { get; set; }
            public bool changePassword { get; set; }
            public bool lockoutEnabled { get; set; }
            public string employeeCode { get; set; }
            public bool active { get; set; }
        }
        public class Query : ICommand<IEnumerable<UserList>>
        {
            public string Username { get; set; }
            public string EmployeeName { get; set; }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<UserList>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<IEnumerable<UserList>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrEmpty(request.EmployeeName))
                {
                    request.EmployeeName = request.EmployeeName.Replace(" ", String.Empty);
                }
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"SELECT 		su.user_id as ""id""
                                              , su.user_name as ""userName""
                                              , concat(get_wording_lang(@lang, de.first_name_th, de.first_name_en), ' ', get_wording_lang(@lang, de.last_name_th, de.last_name_en)) as ""employeeName""
                                              , su.force_change_password as ""changePassword""
                                              , su.lockout_enabled as ""lockoutEnabled""
                                              , su.employee_code as ""employeeCode""
                                              , su.active as ""active""
                                              ,	su.xmin as ""rowVersion""
                                      FROM st_user su
                                      INNER JOIN db_employee de on de.employee_code = su.employee_code
                                      WHERE 1 = 1");
                if (!string.IsNullOrEmpty(request.Username))
                {
                    sql.AppendLine($"AND su.user_name ilike concat('%{request.Username}%') ");
                }
                if (!string.IsNullOrEmpty(request.EmployeeName))
                {
                    if (this._user.Language == "th")
                    {
                        sql.AppendLine($"AND concat(de.first_name_th,de.last_name_th) ilike concat('%{request.EmployeeName}%')  ");
                    }
                    else
                    {
                        sql.AppendLine($"AND concat(de.first_name_en,de.last_name_en) ilike concat('%{request.EmployeeName}%')  ");
                    }
                    
                }
                sql.AppendLine("order by su.user_id");
                IEnumerable<UserList> User = new List<UserList>();
                User =  await _context.QueryAsync<UserList>(sql.ToString(), new
                {
                    lang = this._user.Language,
                }, cancellationToken);
                return User;
            }
        }
    }
}
