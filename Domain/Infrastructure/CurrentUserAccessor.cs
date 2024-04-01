using Application.Interfaces;
using Domain.Types;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private bool IsInHttpContext()
        {
            return _httpContextAccessor.HttpContext != null;
        }

        public int UserId
        {
            get => IsInHttpContext() ? int.Parse(_httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "1") : 1;
        }

        public string UserName
        {
            get => IsInHttpContext() ? _httpContextAccessor.HttpContext.User?.FindFirstValue(JwtClaimTypes.Name) : "system";
        }

        private StringValues _company;
        public string Company
        {
            get
            {
                if (IsInHttpContext()) _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("company", out _company);
                else _company = "01";
                return _company;
            }
        }

        private StringValues _program;
        public string Program
        {
            get
            {
                if (IsInHttpContext()) _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("program", out _program);
                else _program = "system";
                return _program;
            }
        }

        private StringValues _language;
        public string Language {
            get
            {
                if (IsInHttpContext()) _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("language", out _language);
                else _language = "th";
                return _language;
            }
        }
    }
}
