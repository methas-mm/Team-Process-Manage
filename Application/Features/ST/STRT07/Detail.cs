using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ST.STRT07
{
    public class Detail
    {
        public class Query : IRequest<StParameter>
        {
            public string ParameterGroupCode { get; set; }
            public string ParameterCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, StParameter>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<StParameter> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _context.Set<StParameter>().Where(i => i.ParameterGroupCode == request.ParameterGroupCode && i.ParameterCode == request.ParameterCode).FirstOrDefaultAsync(cancellationToken);



                //var data = await _context.Set<StParameter>().Where(c => c.ParameterGroupCode == request.Id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                //    data.StParameterCode = await _context.Set<StParameter>().Where(c => c.ParameterCode == request.Id2).OrderBy(c => c.ParameterGroupCode).AsNoTracking().ToListAsync(cancellationToken);
                

                return data;
            }


        }
    }
}

