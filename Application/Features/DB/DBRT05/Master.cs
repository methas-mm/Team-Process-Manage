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

namespace Application.Features.DB.DBRT05
{
    public class Master
    {
        public class MasterList
        {
            public IEnumerable<dynamic> TeamCode { get; set; }
            public IEnumerable<dynamic> EmployeeCode { get; set; }
        }
        public class Query : IRequest<MasterList>
        {
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
                    sql.AppendLine(@"select dt.team_id as value,
                                    concat(dt.team_code ,' : ',get_wording_lang(@lang,dt.team_name_th ,dt.team_name_eng)) as text
                                    from db_team dt");
                                    
                    master.TeamCode = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                    sql = new StringBuilder();
                    sql.AppendLine(@"select de.employee_code as value,
                                    concat(de.employee_code ,' : ',get_wording_lang(@lang,de.first_name_th ,de.first_name_en)) as text
                                    from db_employee de");

                    master.EmployeeCode = await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language, company = this._user.Company }, cancellationToken);

                    return master;

                }
            }
        }
    }
 }
