using Application.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Demo
{
    public class GetMaster
    {
        public class Data
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class Master
        {
            public List<Data> Genders { get; set; }
            public List<Data> Radios { get; set; }
        }

        public class Query : IRequest<Master>
        {

        }

        public class Handler : IRequestHandler<Query, Master>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Master> Handle(Query request, CancellationToken cancellationToken)
            {
                Master master = new Master();

                List<Data> genders = new List<Data>();
                genders.Add(new Data { Text = "Male", Value = "M" });
                genders.Add(new Data { Text = "Female", Value = "F" });
                master.Genders = genders;

                List<Data> radios = new List<Data>();
                radios.Add(new Data { Text = "Radio 1", Value = "1" });
                radios.Add(new Data { Text = "Radio 2", Value = "2" });
                radios.Add(new Data { Text = "Radio 3", Value = "3" });
                radios.Add(new Data { Text = "Radio 4", Value = "4" });
                master.Radios = radios;

                return master;
            }
        }
    }
}
