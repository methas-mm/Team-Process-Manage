using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Features.ST.STRT05
{
    public class Detail
    {
        public class Query : IRequest<StUserDTO>
        {
            public long Id { get; set; }
        }
        public class StUserDTO : StUser
        {
            public new IEnumerable<StUserProfile> Profiles { get; set; }
        }
        public class Handler : IRequestHandler<Query, StUserDTO>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<StUserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql;
                sql = new StringBuilder();
                sql.AppendLine("select user_id as id,*,xmin as \"rowVersion\" from st_user where user_id = @userId ");
                var user = await _context.QueryFirstOrDefaultAsync<StUserDTO>(sql.ToString(), new { userId = request.Id }, cancellationToken);
                sql = new StringBuilder();
                sql.AppendLine("select user_id as id,*,xmin as \"rowVersion\" from st_user_profile where user_id = @userId");
                user.Profiles = await _context.QueryAsync<StUserProfile>(sql.ToString(), new { userId = request.Id }, cancellationToken);
                return user;
            }
        }
    }

}
