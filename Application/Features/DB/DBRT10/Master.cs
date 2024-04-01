using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> PrefixId { get; set; }
            public IEnumerable<dynamic> Gender { get; set; }
            public IEnumerable<dynamic> PositionId { get; set; }
            public IEnumerable<dynamic> TeamId { get; set; }
            public IEnumerable<dynamic> CompanyNameTh { get; set; }

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
                //master.PrefixId = await _context.Set<DbListItem>().Where(c => c.ListItemGroupCode == "PrefixName" && c.Active == true)
                //       .Select(s => new
                //       {
                //           value = s.ListItemCode,
                //           text = s.ListItemNameTha
                //       }).ToListAsync();

                //master.Gender = await _context.Set<DbListItem>().Where(c => c.ListItemGroupCode == "Gender" && c.Active == true)
                //       .Select(s => new
                //       {
                //           value = s.ListItemCode,
                //           text = s.ListItemNameTha
                //       }).ToListAsync();

                //master.PositionId = await _context.Set<DbListItem>().Where(c => c.ListItemGroupCode == "Position" && c.Active == true)
                //       .Select(s => new
                //       {
                //           value = s.ListItemCode,
                //           text = s.ListItemNameTha
                //       }).ToListAsync();

                StringBuilder sql = new StringBuilder();


                sql = new StringBuilder();
                sql.AppendLine("select team_id as value ,");
                sql.AppendLine("team_name_th as text");
                sql.AppendLine("from db_team dt  ");
                master.TeamId = await _context.QueryAsync<dynamic>(sql.ToString(), cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine("select company_code as value ,");
                sql.AppendLine("company_name_th as text");
                sql.AppendLine("from st_company  ");
                master.CompanyNameTh = await _context.QueryAsync<dynamic>(sql.ToString(), cancellationToken);

                return master;
            }
        }
    }
}
