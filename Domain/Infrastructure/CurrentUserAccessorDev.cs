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
    public class CurrentUserAccessorDev : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessorDev(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get => 2;

        }
        public string UserName
        {
            get => "admin";
        }

        private StringValues _company;
        public string Company
        {
            get
            {
                return "001";
            }
        }

        private StringValues _program;
        public string Program
        {
            get
            {
                return "system";
            }
        }
        private StringValues _language;
        public string Language
        {
            get
            {
                return "th";
            }
        }
    }
}
