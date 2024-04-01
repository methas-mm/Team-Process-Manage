using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Personal
{
    public class Organization
    {

        public class OrganizationVm
        {
            public IEnumerable<dynamic> Companies { get; set; }
            public IEnumerable<dynamic> Divisions { get; set; }
        }

        public class Query : IRequest<OrganizationVm>
        {
            public string Name { get; set; }

            public string Lang { get; set; }
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, OrganizationVm>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<OrganizationVm> Handle(Query request, CancellationToken cancellationToken)
            {
                var organize = new OrganizationVm();

                if (request.Name == "company")
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"select c.company_code  as value,case 'th' when 'th' then company_name_th else coalesce(company_name_eng ,company_name_th) end as text
                                    from st_company c 
                                    inner join db_employee de  on de.company_code  = c.company_code  
                                    inner join st_user su on su.employee_code = de.employee_code 
                                    where user_id  = @UserId");
                    organize.Companies = await _context.QueryAsync<dynamic>(sql.ToString(), new { UserId = _user.UserId, Lang = request.Lang }, cancellationToken);
                }
                return organize;
            }
        }
    }
}
