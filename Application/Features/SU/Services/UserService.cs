using Application.Background;
using Application.Common.Constants;
using Application.Common.Helpers;
using Application.Common.Models;
using Application.Exceptions;
using Application.Hubs;
using Application.Interfaces;
using Application.Interfaces.Email;
using Domain.Entities.SU;
using Domain.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Services
{
    public class Result
    {
        public long Id { get; set; }
        public bool haveEmail { get; set; }
    }

    public class UserService : IService
    {
        private readonly ICleanDbContext _context;
        private readonly ICurrentUserAccessor _user;
        private readonly IIdentityService _identity;
        private readonly IEmailSender _email;
        private readonly ILogger<UserService> _logger;
        private readonly IHubContext<MigrateJobHub> _jobHub;
        protected virtual CancellationToken CancellationToken => default;

        public UserService(ICleanDbContext context, ICurrentUserAccessor user, IIdentityService identity, IEmailSender email, ILogger<UserService> logger, IHubContext<MigrateJobHub> jobHub)
        {
            _context = context;
            _user = user;
            _identity = identity;
            _email = email;
            _logger = logger;
            _jobHub = jobHub;
        }

        public async Task UpdateStudentUserEndDate(long studentId, DateTime endDate)
        {
            var userType = await _context.Set<SuUserType>().AsNoTracking().FirstOrDefaultAsync(o => o.StudentId == studentId && o.UserType == UserType.Student, CancellationToken);
            if (userType == null)
            {
                throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
            }
            await this.UpdateUserEndDate(userType.UserId, endDate);
        }
        public async Task UpdateEmployeeUserEndDate(string companyCode, string employeeCode, DateTime endDate)
        {
            var userType = await _context.Set<SuUserType>().AsNoTracking().FirstOrDefaultAsync(o => o.CompanyCode == companyCode && o.EmployeeCode == employeeCode && o.UserType != UserType.Student, CancellationToken);
            if (userType == null)
            {
                throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
            }
            await this.UpdateUserEndDate(userType.UserId, endDate);
        }

        private async Task UpdateUserEndDate(long userId, DateTime endDate)
        {
            var user = await _context.Set<SuUser>().FindAsync(userId);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
            }
            user.EndEffectiveDate = endDate;
            await _context.SaveChangesAsync(CancellationToken);
        }

        private async Task AddDivision(ICollection<SuUserDivision> userDivisions,string company,string divParent)
        {
            if (!string.IsNullOrEmpty(divParent))
            {
                var isExist = await _context.Set<SuDivision>().AnyAsync(o => o.CompanyCode == company && o.DivCode == divParent);
                if (isExist && !userDivisions.Any(p=>p.DivCode == divParent))
                {
                    userDivisions.Add(new SuUserDivision() { DivCode = divParent });
                    var children = await _context.Set<SuDivision>().Where(o => o.DivParent == divParent).AsNoTracking().ToListAsync(CancellationToken);
                    foreach (var div in children)
                    {
                        await AddDivision(userDivisions, company, div.DivCode);
                    }
                }
              
            }
        }
    }
}
