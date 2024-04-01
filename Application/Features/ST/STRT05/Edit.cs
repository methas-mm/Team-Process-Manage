using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Exceptions;
using System.Net;
using Domain.Entities;

namespace Application.Features.ST.STRT05
{
    public class Edit
    {
        public class Command : StuserDTO, ICommand<long>
        {

        }
        public class StuserDTO : StUser
        {
            public bool chagePassword { get; set; }
        }
        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ICleanDbContext _context;
            private readonly IIdentityService _identity;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, IIdentityService identity, ICurrentUserAccessor user)
            {
                _context = context;
                _identity = identity;
                _user = user;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Set<StUser>().Where(o => o.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
                _context.Entry(user).Property("RowVersion").OriginalValue = request.RowVersion;
                user.Active = request.Active;
                user.DefaultLang = request.DefaultLang;
                user.PasswordPolicyCode = request.PasswordPolicyCode;
                user.StartEffectiveDate = request.StartEffectiveDate;
                user.EndEffectiveDate = request.EndEffectiveDate;
                user.UpdatedBy = _user.UserName;
                user.UpdatedDate = DateTime.Now;
                var result = await _identity.UpdateUserAsync(user);


                if (!result.Succeeded)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, String.Join(",", result.Errors));
                }

                if (request.chagePassword)
                {
                    user.PasswordHash = request.PasswordHash;
                    user.LastChangePassword = DateTime.Now;
                    var ChangePassWordresult = await _identity.ChangePasswordAsync(user, request.PasswordHash);
                }

                _context.Set<StUserProfile>().RemoveRange(request.Profiles.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.Profiles = request.Profiles.Where(o => o.RowState != RowState.Delete).ToList();

                _context.Set<StUserProfile>().AddRange(request.Profiles);
                await _context.SaveChangesAsync(cancellationToken);

                return user.Id;
            }
        }
    }
}
