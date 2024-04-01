using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT15
{
    public class Detail
    {
        public class Query : IRequest<DbHoliday>
        {
            public int? holidayId { get; set; }
            public string? holidayName { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbHoliday>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<DbHoliday> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _context.Set<DbHoliday>().Where(c => c.HolidayId == request.holidayId).FirstOrDefaultAsync(cancellationToken);
                if (!string.IsNullOrEmpty(request.holidayName))
                {
                    data = await _context.Set<DbHoliday>().Where(c => c.HolidayName == request.holidayName).FirstOrDefaultAsync(cancellationToken);
                }
                return data;
            }


        }
    }
}
