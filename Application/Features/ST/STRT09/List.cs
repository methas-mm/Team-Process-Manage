using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT09
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
                sql.AppendLine(@"select dlig.list_item_group_code as groupId ,dlig.list_item_group_name as groupName , list_item_code as listId , get_wording_lang('th',list_item_name_tha,list_item_name_eng) as listName , dli.xmin as ""rowVersion"" from db_list_item dli
                                   inner join db_list_item_group dlig on dlig.list_item_group_code = dli.list_item_group_code");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine($@"where concat( dlig.list_item_group_code  ,dlig.list_item_group_name  , list_item_code  , get_wording_lang('th',list_item_name_tha,list_item_name_eng) ) ilike concat('%{request.Keyword}%')");
                }
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = this._user.Language }, cancellationToken);

            }
        }

    }
}
