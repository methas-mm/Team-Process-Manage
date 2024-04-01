using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT03
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
        }

        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT      profile_code AS \"profileCode\",");
                sql.AppendLine("            profile_desc AS \"profileDesc\",");
                sql.AppendLine("            active,");
                sql.AppendLine("            xmin AS \"rowVersion\"");
                sql.AppendLine("FROM        st_profile");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("WHERE   CONCAT(profile_code,");
                    sql.AppendLine("               profile_desc)");
                    sql.AppendLine("        ILIKE CONCAT('%', @Keyword, '%')");
                }
                sql.AppendLine("order by profile_code");

                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
