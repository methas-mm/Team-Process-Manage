using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT05
{
    public class Master
    {
        public class MasterList
        {
            public IEnumerable<dynamic> WorkGroup { get; set; }
            public IEnumerable<dynamic> WorkCode { get; set; }
            public IEnumerable<dynamic> Employee { get; set; }
            public IEnumerable<dynamic> Position { get; set; }
            public IEnumerable<dynamic> Years { get; set; }
            public IEnumerable<dynamic> Phrase { get; set; }
        }
        public class Query : IRequest<MasterList>
        {

        }
        public class ProfileDTO
        {
           
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
                master.WorkGroup = await getWorkGroup(cancellationToken);
                //master.WorkCode = await getWork(cancellationToken);
                master.Employee = await getEmployee(cancellationToken);
                master.Position = await getPosition(cancellationToken);
                master.Years = GetYear();
                master.Phrase = await GetPhrase(cancellationToken);
                return master;
            }
            public async Task<IEnumerable<dynamic>> getWorkGroup(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select workcode_group_id as value,workcode_group_code || ' : ' || get_wording_lang(@lang,workgroup_name_th,workgroup_name_en) as text from pm_workcode_group where active = true order by workcode_group_code");
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
            public async Task<IEnumerable<dynamic>> getWork(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select workcode_id as value,workcode_code || ' : ' || get_wording_lang(@lang,workcode_name_th,workcode_name_en) as text from pm_workcode order by workcode_code ");
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
            public async Task<IEnumerable<dynamic>> getEmployee(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select employee_code as value,concat(employee_code,' : ',get_wording_lang(@lang,first_name_th ,first_name_en),' ',get_wording_lang(@lang,last_name_th ,last_name_en)) as text from db_employee order by text");
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }

            public async Task<IEnumerable<dynamic>> getPosition(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select list_item_code as value, get_wording_lang(@lang,list_item_name_tha,list_item_name_eng) as text from db_list_item where list_item_group_code = 'Position' and active = true");
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }

            private IEnumerable<dynamic> GetYear()
            {
                var year = int.Parse(DateTime.Today.ToString("yyyy"));
                var yearsRange = Enumerable.Range(year - 10, 10).Concat(Enumerable.Range(year, 6));
                return yearsRange.Select(o => new { value = o, text = o }).OrderByDescending(y => y.value);
            }

            private async Task<IEnumerable<dynamic>> GetPhrase(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select dli.list_item_code as value , get_wording_lang(@lang,dli.list_item_name_tha,dli.list_item_name_eng) as text from db_list_item dli where dli.list_item_group_code = 'Phrase' and dli.active = true");
                return await _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
        }
    }
}
