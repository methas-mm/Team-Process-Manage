using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense
{
    public class ReplicaDbContext : CleanDbContext, IReplicaOneDbContext, IReplicaTwoDbContext
    {
        public ReplicaDbContext(DbContextOptions<ReplicaDbContext> options, ICurrentUserAccessor currentUserAccessor) : base(options, currentUserAccessor)
        {
        }
    }
}