using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities.ST;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Identity
{
    public class IdentityService : IIdentityService
    {
        //private readonly UserManager<SuUser> _userManager;
        private readonly UserManager<StUser> _userManager;

        public IdentityService( UserManager<StUser> userManager)
        {
            //_userManager = userManager;
            _userManager = userManager;
        }

        public async Task<string> GetUserNameAsync(long userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<(Result Result, long UserId)> CreateUserAsync(StUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> DeleteUserAsync(long userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(StUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<Result> UpdateUserAsync(StUser user)
        {
            var result = await _userManager.UpdateSecurityStampAsync(user);
            return result.ToApplicationResult();
        }
        public string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
        public async Task<Result> ChangePasswordAsync(StUser suUser, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(suUser.Id.ToString());
            var resultRemove = await _userManager.RemovePasswordAsync(user);
            var resultAdd = await _userManager.AddPasswordAsync(user, newPassword);
            return resultAdd.ToApplicationResult();
        }

    }
}
