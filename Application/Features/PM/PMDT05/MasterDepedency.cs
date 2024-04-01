using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class MasterDepedency
    {
        public class MasterList
        {
            public IEnumerable<dynamic> WorkCode { get; set; }
            public IEnumerable<dynamic> Employee { get; set; }
        }
        public class Query : IRequest<MasterList>
        {
            public string Field { get; set; }
            public string Value { get; set; }
        }
        public class Handler : IRequestHandler<Query, MasterList>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MasterList> Handle(Query request, CancellationToken cancellationToken)
            {
                MasterList master = new MasterList();
                switch (request.Field)
                {
                    case "workCode":
                        master.WorkCode = await _context.QueryAsync<dynamic>(@"select pw.workcode_id as value, pw.workcode_code || ' : ' || get_wording_lang(@lang,pw.workcode_name_th,pw.workcode_name_en) as text from pm_workcode pw where pw.workcode_group_id = @workGroupId order by pw.workcode_code", new { lang = _user.Language, workGroupId = int.Parse(request.Value) }, cancellationToken);
                        break;
                    case "employee":
                        master.Employee = await _context.QueryAsync<dynamic>(@"select de.employee_code as value,de.employee_code || ' : ' || get_full_employee_name(@lang,de.employee_code) as text from db_employee de where de.position_id = @Value order by text", new { lang = _user.Language, Value = request.Value }, cancellationToken);
                        break;
                }

                return master;
            }
        }
    }
}
