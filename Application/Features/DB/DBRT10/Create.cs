using Domain.Entities.DB;
using Application.Behaviors;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Application.Exceptions;

namespace Application.Features.DB.DBRT10
{
   public class Create 
    {
        public class Command : DbEmployee, ICommand<DbEmployee>
        {
            //public int? SubDistrictId { get; set; }
            //public int? DistrictId { get; set; }
            //public int? ProvinceId { get; set; }
            //public int? PostalCode { get; set; }
        }
        public class Handler : IRequestHandler<Command, DbEmployee>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<DbEmployee> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbEmployee>().Any(i => i.EmployeeCode == request.EmployeeCode ))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014" , "label.DBRT10.EmployeeCode ");

                _context.Set<DbEmployee>().Add((DbEmployee)request);
                await _context.SaveChangesAsync(cancellationToken);

                return request;
            }
        }
    }
}
