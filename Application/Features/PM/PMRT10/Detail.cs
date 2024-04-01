using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PM.PMRT10
{
    public class Detail
    {
        public class Query : IRequest<PmCustomer>
        {
            public string CustomerCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, PmCustomer>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<PmCustomer> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _context.Set<PmCustomer>().Where(c => c.CustomerCode == request.CustomerCode).FirstOrDefaultAsync(cancellationToken);
                return data;
            }


        }
    }
}