using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.ST.STRT07
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
                sql.AppendLine("select");
                sql.AppendLine(" sp.parameter_group_code as \"parameterGroupCode\", ");
                sql.AppendLine(" sp.parameter_code as \"parameterCode\", ");
                sql.AppendLine(" sp.parameter_value as \"parameterValue\", ");
                sql.AppendLine(" sp.remark ,");
                sql.AppendLine(" sp.xmin AS rowVersion");
                sql.AppendLine(" from st_parameter sp ");

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine("WHERE       CONCAT(sp.parameter_group_code,");
                    sql.AppendLine("                   sp.parameter_code,");
                    sql.AppendLine("                   sp.parameter_value,");
                    sql.AppendLine("                   sp.remark)");
                    sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                }
                sql.AppendLine(" order by sp.parameter_group_code");

                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }


        }
        
    }
}
