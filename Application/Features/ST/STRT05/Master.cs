using Application.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.ST;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT05
{
    public class Master
    {
        public class MasterList
        {
            public IEnumerable<dynamic> EmployeeName { get; set; }
            public IEnumerable<dynamic> UserProfiles { get; set; }
        }
        public class Query : IRequest<MasterList>
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
                master.EmployeeName = await _context.Set<DbEmployee>().OrderBy(o => o.EmployeeCode).Select(s => new
                {
                    value = s.EmployeeCode,
                    text = this._user.Language == "th" ? s.EmployeeCode + " : " + s.FirstNameTh + " " + s.LastNameTh : s.EmployeeCode + " : " + s.FirstNameEn + " " + s.LastNameEn
                }).ToListAsync(cancellationToken);
                master.UserProfiles = await _context.Set<StProfile>().OrderBy(o => o.ProfileCode).Where(i => i.Active == true).Select(s => new
                {
                    value = s.ProfileCode,
                    text = String.Concat(s.ProfileCode, " : ", s.ProfileDesc)
                }).ToListAsync(cancellationToken);
                return master;
            }
        }
    }
}
