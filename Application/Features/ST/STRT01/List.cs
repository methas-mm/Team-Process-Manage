using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT01
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
                sql.AppendLine(@"select company_code as companyCode, get_wording_lang(@lang,company_name_th ,company_name_eng ) as companyName , active , xmin AS ""rowVersion"" from st_company  ");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine($@"where  concat( company_code , get_wording_lang('th',company_name_th ,company_name_eng ) , active ) ilike concat('%{request.Keyword}%')");
                }
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

            }
        }
    }
}
