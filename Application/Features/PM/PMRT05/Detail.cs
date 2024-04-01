using Application.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT05
{
    public class Detail
    {
        public class Query : IRequest<PmWorkcode>
        {
            public int Id { get; set; }
            public int GroupId { get; set; }
        }
        public class Handler : IRequestHandler<Query, PmWorkcode>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<PmWorkcode> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"
                                    select pwg.workcode_group_code as ""workgroupcode"",
                                     pwg.workcode_group_id as ""WorkcodeGroupId"",
                                     pw.workcode_id as ""workcodeId"",
                                     pwg.workgroup_name_th as ""workgroupNameTh"",
                                     pw.workcode_name_en as ""workgroupNameEn"",
                                     pw.workcode_code as ""workcodeCode"",
                                     pw.workcode_name_th as ""workcodeNameTh"",
                                     pw.xmin as ""rowVersion""
                                    from pm_workcode pw
                                    left JOIN pm_workcode_group pwg on pwg.workcode_group_id = pw.workcode_group_id
                                    where pw.workcode_id = @Id");
                PmWorkcode data = await _context.QueryFirstAsync<PmWorkcode>(sql.ToString(), new { Id = request.Id }, cancellationToken);
                return data;
            }


        }
    }
}