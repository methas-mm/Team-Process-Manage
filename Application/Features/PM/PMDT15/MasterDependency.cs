using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT15
{
    public class MasterDependency
    {
        public class MasterList
        {
            public IEnumerable<dynamic> ProjectName { get; set; }
        }
        public class Query : IRequest<MasterList>
        {
            public string Field { get; set; }
            public int? Value { get; set; }
        }
        public class Handler : IRequestHandler<Query, MasterList>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MasterList> Handle(Query request, CancellationToken cancellationToken)
            {
                MasterList master = new MasterList();
                StringBuilder sql;
                switch (request.Field)
                {

                }

                return master;
            }
        }
    }
}
