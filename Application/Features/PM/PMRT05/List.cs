using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT05
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get;  set; }
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
                    sql.AppendLine(@"select pwg.workcode_group_code as ""workcodeGroupCode"",
                                     pwg.workcode_group_id as ""WorkcodeGroupId"",
                                     pwg.workgroup_name_th as""workgroupNameTh"",
                                     pw.workcode_id as ""workcodeId"",
                                     pw.workcode_name_en as ""workcodeNameEn"",
                                     pw.workcode_code as ""workcodeCode"",
                                     pw.workcode_name_th as ""workcodeNameTh"",
                                     pw.xmin as ""rowVersion""
                                    from pm_workcode pw
                                    left JOIN pm_workcode_group pwg on pwg.workcode_group_id = pw.workcode_group_id
                                    where pw.workcode_group_id = pw.workcode_group_id");
                    if (!string.IsNullOrEmpty(request.Keyword))
                    {
                        {
                            sql.AppendLine("and        CONCAT(pwg.workcode_group_code,");
                            sql.AppendLine("                   pwg.workgroup_name_th,");
                            sql.AppendLine("                   pw.workcode_name_th,");
                            sql.AppendLine("                   pw.workcode_code)");
                            sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                        }
                        sql.AppendLine(" order by pwg.workcode_group_code"); ;
                    }

                    return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);

                }
            }
        }
    }


