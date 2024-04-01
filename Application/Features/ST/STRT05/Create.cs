using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT05
{
    public class Create
    {
        public class Command : StUser, ICommand<long>
        {

        }
        public class Handler : IRequestHandler<Command, long>
        {
            CultureInfo culture = new CultureInfo("en-US");

            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            private readonly IIdentityService _identity;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user, IIdentityService identity)
            {
                _context = context;
                _user = user;
                _identity = identity;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<StUser>().Any(o => o.UserName.Trim().ToLower() == request.UserName.Trim().ToLower()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00014", "label.STRT05.Username");
                }

                StUser user = new StUser();

                user.UserName = request.UserName;
                user.EmployeeCode = request.EmployeeCode;
                // Static Value
                user.PasswordPolicyCode = "000";

                user.StartEffectiveDate = request.StartEffectiveDate;
                user.EndEffectiveDate = request.EndEffectiveDate;
                

                var result = await _identity.CreateUserAsync(user, request.PasswordHash);

                user.Active = request.Active;
                user.LockoutEnabled = false;
                user.LockoutEnd = DateTime.Now;
                user.CreatedBy = this._user.UserName;
                user.CreatedDate = DateTime.Now;

                //foreach (var item in request.Profiles)
                //{
                //    item.Id = user.Id;
                //}
                user.Profiles = request.Profiles;
                _context.Set<StUser>().Attach(user);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                return user.Id;
            }
        }
    }
}
