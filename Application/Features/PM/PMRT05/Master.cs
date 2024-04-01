using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMRT05
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> GroupCode { get; set; }
        }
        public class Query : IRequest<MasterData>
        {

        }
        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

                public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
                {
                    var master = new MasterData();
  

                    StringBuilder sql = new StringBuilder();


                    sql = new StringBuilder();
                    sql.AppendLine("select workcode_group_id as value ,");
                    sql.AppendLine("concat(workcode_group_code ,' : ', workgroup_name_th )as text");
                    sql.AppendLine("from pm_workcode_group pwg   ");
                    master.GroupCode = await _context.QueryAsync<dynamic>(sql.ToString(),new { }, cancellationToken);

               



                return master;
                }
            }
        }
    }
