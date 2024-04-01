using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.EA;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EA.EADT03
{
    public class Create
    {
        public class Command : EaEvaluate, ICommand<int?>
        {

        }
        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<EaEvaluate>().Any(i => i.EmployeeCode == request.EmployeeCode && i.Year == request.Year))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "แบบประเมิน");
                EaEvaluate form = new EaEvaluate();
                form.CompetitionFormId = request.CompetitionFormId;
                form.EmployeeCode = request.EmployeeCode;
                form.Year = request.Year;
                form.status = request.status;
                _context.Set<EaEvaluate>().Add((EaEvaluate)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.EvaluateId;
            }
        }
    }
}
