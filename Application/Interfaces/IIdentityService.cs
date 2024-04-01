using Application.Common.Models;
using Domain.Entities.ST;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(long userId);

        Task<(Result Result, long UserId)> CreateUserAsync(StUser user, string password);

        Task<Result> DeleteUserAsync(long userId);

        Task<Result> UpdateUserAsync(StUser user);

        Task<Result> ChangePasswordAsync(StUser user,string newPassword);
        
        string GeneratePassword();
    }
}
