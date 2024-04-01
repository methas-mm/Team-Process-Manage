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
using Application.Exceptions;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT15
{
    public class Copy
    {
        public class Command : DbHoliday, ICommand
        {
            public string YearForm { get; set; }
            public string YearTo { get; set; }

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                List<DbHoliday> itemh = _context.Set<DbHoliday>().Where(w => w.HolidayEndDate.Value.Year == int.Parse(request.YearForm) - 543).OrderBy(o => o.HolidayEndDate).ToList();
                List<DbHoliday> itemYt = _context.Set<DbHoliday>().Where(w => w.HolidayEndDate.Value.Year == int.Parse(request.YearTo) - 543).OrderBy(o => o.HolidayEndDate).ToList();
                List<DbHoliday> data = _context.Set<DbHoliday>().Where(w => w.HolidayDate.Value.Year == int.Parse(request.YearForm) - 543).OrderBy(o => o.HolidayDate).ToList();
                List<DbHoliday> dataYt = _context.Set<DbHoliday>().Where(w => w.HolidayDate.Value.Year == int.Parse(request.YearTo) - 543).OrderBy(o => o.HolidayDate).ToList();

                foreach (DbHoliday datayt in dataYt )
                {
                    if (_context.Set<DbHoliday>().Any(i => (i.HolidayDate.Value.Day + i.HolidayDate.Value.Month).ToString() == (datayt.HolidayDate.Value.Day + datayt.HolidayDate.Value.Month).ToString() && i.HolidayDate.Value.Year == int.Parse(request.YearTo) - 543) )
                        {
                        _context.Set<DbHoliday>().Attach((DbHoliday)datayt);
                        _context.Entry((DbHoliday)datayt).State = EntityState.Modified;
                        data.Remove(data.Where(w => (w.HolidayDate.Value.Day + w.HolidayDate.Value.Month).ToString()  == (datayt.HolidayDate.Value.Day + datayt.HolidayDate.Value.Month).ToString() ).FirstOrDefault() );
                    }
                }

                foreach (DbHoliday item in data)
           
                {
                    DbHoliday dbHoliday = new DbHoliday();
                    dbHoliday.HolidayDate = new DateTime(int.Parse(request.YearTo) - 543, item.HolidayDate.Value.Month, item.HolidayDate.Value.Day);
                    if(item.HolidayEndDate != null)
                    {
                        dbHoliday.HolidayEndDate = new DateTime(int.Parse(request.YearTo) - 543, item.HolidayEndDate.Value.Month, item.HolidayEndDate.Value.Day);
                    }
                    dbHoliday.HolidayDesc = item.HolidayDesc;
                    dbHoliday.HolidayName = item.HolidayName;
                    dbHoliday.Active = true;


                    _context.Set<DbHoliday>().Add(dbHoliday);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return Unit.Value;
            }
        }
    }
}