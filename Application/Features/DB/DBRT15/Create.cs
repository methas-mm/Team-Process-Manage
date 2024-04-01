using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT15
{
    public class Create
    {
        public class Command : DbHoliday, ICommand
        {
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbHoliday>().Any(i => i.HolidayDate == request.HolidayDate))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00019");

                if (_context.Set<DbHoliday>().Any(i => i.HolidayDate <= request.HolidayDate && i.HolidayEndDate >= request.HolidayDate ))
                throw new RestException(HttpStatusCode.BadRequest, "message.STD00019");
                
                
                if ( request.HolidayEndDate != null)
                {
                    if (_context.Set<DbHoliday>().Any(i => i.HolidayEndDate >= request.HolidayDate && i.HolidayDate < request.HolidayEndDate))
                        throw new RestException(HttpStatusCode.BadRequest, "message.STD00019");

                    

                    if (_context.Set<DbHoliday>().Any(i => i.HolidayDate == request.HolidayEndDate))
                        throw new RestException(HttpStatusCode.BadRequest, "message.STD00019");
                }
             

                _context.Set<DbHoliday>().Add((DbHoliday)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
