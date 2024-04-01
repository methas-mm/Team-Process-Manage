using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT09
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> ListGroup { get; set; }

        }
        public class Query : IRequest<MasterData>
        {

        }
        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                 _context = context;
                _user = user;
            }

            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                var master = new MasterData();

                StringBuilder sql = new StringBuilder();
                sql = new StringBuilder();
                sql.AppendLine("select list_item_group_code as value  , concat(list_item_group_code , ' : ' , list_item_group_name )as text  from db_list_item_group dlig");
                master.ListGroup = await _context.QueryAsync<dynamic>(sql.ToString(),new { }, cancellationToken);

                return master;
            }
        }

    }
}
