using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT03
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {

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
                sql.AppendLine(" pwg.workcode_group_id as \"workcodeGroupId\", ");
                sql.AppendLine(" pwg.workcode_group_code as \"workcodeGroupCode\", ");
                sql.AppendLine(" pwg.workgroup_name_th as \"workgroupNameTh\", ");
                sql.AppendLine(" pwg.workgroup_name_en as \"workgroupNameEn\", ");
                sql.AppendLine(" pwg.active,");
                sql.AppendLine(" pwg.xmin as \"rowVersion\"");
                sql.AppendLine(" from pm_workcode_group pwg");
                sql.AppendLine(" order by pwg.workcode_group_code ");

                

                return await _context.GetPage(sql.ToString(), null, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
