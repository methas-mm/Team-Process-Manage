using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT09
{
    public class Create
    {
        public class Command : DbListItem, ICommand<DbListItem>
        {

        }

        public class Handler : IRequestHandler<Command, DbListItem>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;

            }

            public async Task<DbListItem> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbListItem>().Any(i => i.ListItemCode == request.ListItemCode))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014");

                _context.Set<DbListItem>().Add((DbListItem)request);
                await _context.SaveChangesAsync(cancellationToken);

                return request;
            }
        }
    }
}
