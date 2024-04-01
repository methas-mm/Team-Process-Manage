using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT10
{
   public class Detail
    {
        public class Query : IRequest<DbEmployee>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbEmployee>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async  Task<DbEmployee> Handle(Query request, CancellationToken cancellationToken)
            {
                DbEmployee data = await _context.Set<DbEmployee>().Where(c => c.EmployeeCode == request.Id).FirstOrDefaultAsync(cancellationToken);
                return data ;
            }


        }

    }
}
